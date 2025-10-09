using DesktopBeatLight.Audio.Abstractions;
using NAudio.Wave;
using System;
using System.Threading.Tasks;

namespace DesktopBeatLight.Audio.Implementations;

public class WasapiAudioCapture : IAudioCapture
{
    // NAudio ����Ƶ�������
    private WasapiLoopbackCapture? _capture;
    // ��Ƶ���ݿ����¼�
    public event Action<ReadOnlySpan<float>>? AudioDataAvailable;
    // �Ƿ���
    public bool IsMuted { get; private set; }

    // ��ʼ������Ƶ
    public async Task StartCapture()
    {
        // ֹͣ���в���
        if (_capture != null)
        {
            await StopCapture();
        }
        // �����µĲ���ʵ��
        _capture = new WasapiLoopbackCapture();

        // ע�����ݻص�
        _capture.DataAvailable += OnDataAvailable;

        // ��ʼ����
        _capture.StartRecording();
        await Task.Yield(); // �첽����
    }
    // ���ݿ���ʱ�Ļص�
    private void OnDataAvailable(object? sender, WaveInEventArgs e)
    {
        if (_capture == null) return;

        // ת��Ϊ float ����
        var format = _capture.WaveFormat;
        int floatCount = e.BytesRecorded / 4; // ������ 32-bit float
        float[] floatBuffer = new float[floatCount];
        Buffer.BlockCopy(e.Buffer, 0, floatBuffer, 0, e.BytesRecorded);

        AudioDataAvailable?.Invoke(floatBuffer);
    }
    // ֹͣ������Ƶ
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
    // �ͷ���Դ
    public Task Dispose() => StopCapture();
    // ��ȡƵ�����ݣ���ʵ�֣�ʵ��Ӧ������ҪFFT�ȷ�����
    public Task<float[]> GetSpectrumData()
    {
        // ��ʵ�֣�ʵ��Ӧ������Ҫ����FFT��Ƶ�׷���
        return Task.FromResult(new float[128]);
    }
}