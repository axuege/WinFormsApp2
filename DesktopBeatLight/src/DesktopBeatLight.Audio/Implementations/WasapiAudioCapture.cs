using DesktopBeatLight.Audio.Abstractions;
using NAudio.Wave;
using System;
using System.Threading.Tasks;

namespace DesktopBeatLight.Audio.Implementations;

public class WasapiAudioCapture : IAudioCapture
{
    // NAudio 的音频捕获对象
    private WasapiLoopbackCapture? _capture;
    // 音频数据可用事件
    public event Action<ReadOnlySpan<float>>? AudioDataAvailable;
    // 是否静音
    public bool IsMuted { get; private set; }

    // 开始捕获音频
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
    // 数据可用时的回调
    private void OnDataAvailable(object? sender, WaveInEventArgs e)
    {
        if (_capture == null) return;

        // 转换为 float 数组
        var format = _capture.WaveFormat;
        int floatCount = e.BytesRecorded / 4; // 假设是 32-bit float
        float[] floatBuffer = new float[floatCount];
        Buffer.BlockCopy(e.Buffer, 0, floatBuffer, 0, e.BytesRecorded);

        AudioDataAvailable?.Invoke(floatBuffer);
    }
    // 停止捕获音频
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
    // 释放资源
    public Task Dispose() => StopCapture();
    // 获取频谱数据（简单实现，实际应用中需要FFT等分析）
    public Task<float[]> GetSpectrumData()
    {
        // 简单实现，实际应用中需要进行FFT等频谱分析
        return Task.FromResult(new float[128]);
    }
}