using System;
using NAudio.Wave;

namespace DesktopBeatLight.Audio.Abstractions;

public interface IAudioCapture : IDisposable
{
    /// <summary>
    /// 开始音频捕获
    /// </summary>
    Task StartCapture();
    
    /// <summary>
    /// 停止音频捕获
    /// </summary>
    Task StopCapture();
    
    /// <summary>
    /// 当前是否处于静音状态
    /// </summary>
    bool IsMuted { get; }
    
    /// <summary>
    /// 音频数据可用事件
    /// </summary>
    event Action<ReadOnlySpan<float>>? AudioDataAvailable;
    
    /// <summary>
    /// 将字节数据转换为单声道浮点数据
    /// </summary>
    /// <param name="buffer">音频缓冲区</param>
    /// <param name="bytesRecorded">记录的字节数</param>
    /// <param name="format">音频格式</param>
    /// <returns>转换后的单声道浮点数据</returns>
    Task<float[]> ConvertToMonoFloatAsync(byte[] buffer, int bytesRecorded, WaveFormat format);
}