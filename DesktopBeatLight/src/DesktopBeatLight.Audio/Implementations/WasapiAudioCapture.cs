using DesktopBeatLight.Audio.Abstractions;
using NAudio.Wave;
using System;
using System.Threading.Tasks;
using DesktopBeatLight.Audio.Helpers;

namespace DesktopBeatLight.Audio.Implementations;

public class WasapiAudioCapture : IAudioCapture
{
    // NAudio 音频捕获对象
    private WasapiLoopbackCapture? _capture;
    // 音频数据事件订阅
    public event Action<ReadOnlySpan<float>>? AudioDataAvailable;
    // 是否静音
    public bool IsMuted { get; private set; }
    // 静音检测阈值设为-40 dBFS
    private readonly SilenceDetector _silenceDetector = new SilenceDetector(-40);
    // 静音帧计数器
    private int _silentFrameCount = 0;

    // 开始音频捕获
    public async Task StartCapture()
    {
        // 停止已有捕获
        if (_capture != null)
        {
            await StopCapture();
        }
        // 创建新的捕获实例
        _capture = new WasapiLoopbackCapture();

        // 注册数据回调
        _capture.DataAvailable += OnDataAvailable;

        // 开始捕获
        _capture.StartRecording();
        await Task.Yield(); // 异步返回
    }
    // 数据到达时的回调
    private async void OnDataAvailable(object? sender, WaveInEventArgs e)
    {
        // 空检查
        if (_capture == null) return;

        // 转换为单声道浮点数据
        var floatBuffer = await ConvertToMonoFloatAsync(e.Buffer, e.BytesRecorded, _capture.WaveFormat);

        // 静音检测
        bool isSilent = _silenceDetector.IsSilent(floatBuffer);
        if (isSilent)
        {
            _silentFrameCount++;
            IsMuted = _silentFrameCount >= 100; // 100帧*20ms=2秒
        }
        else
        {
            _silentFrameCount = 0;
            IsMuted = false;
        }

        // 触发音频数据事件供FFT处理使用
        AudioDataAvailable?.Invoke(floatBuffer);
    }
    // 停止音频捕获
    public async Task StopCapture()
    {
        if (_capture != null)
        {
            _capture.DataAvailable -= OnDataAvailable;
            _capture.StopRecording();
            _capture.Dispose();
            _capture = null;
        }
        await Task.Yield();
    }
    // 释放资源 - 显式实现IDisposable接口
    public async void Dispose()
    {
        await StopCapture();
        GC.SuppressFinalize(this);
    }
    
    // 将字节数据转换为单声道浮点数据 - 公开异步方法实现接口
    public Task<float[]> ConvertToMonoFloatAsync(byte[] buffer, int bytesRecorded, WaveFormat format)
    {
        // 使用Task.Run在后台线程执行CPU密集型操作
        return Task.Run(() =>
        {
            int sampleCount = bytesRecorded / 2; // 每样本2字节
            float[] samples = new float[sampleCount];
            
            // 将字节数据转换为浮点数组
            for (int i = 0; i < sampleCount; i++)
            {
                samples[i] = BitConverter.ToInt16(buffer, i * 2) / 32768f; // 归一化到-1~1
            }

            // 如果是立体声，将左右声道混合为单声道
            if (format.Channels == 2)
            {
                float[] monoSamples = new float[sampleCount / 2];
                for (int i = 0; i < monoSamples.Length; i++)
                {
                    monoSamples[i] = (samples[i * 2] + samples[i * 2 + 1]) / 2; // 声道平均
                }
                return monoSamples;
            }
            
            return samples;
        });
    }

    // 为了兼容旧代码，可以保留同步版本但标记为过时
    [Obsolete("请使用ConvertToMonoFloatAsync代替")]
    public float[] ConvertToMonoFloat(byte[] buffer, int bytesRecorded, WaveFormat format)
    {
        return ConvertToMonoFloatAsync(buffer, bytesRecorded, format).Result;
    }
}