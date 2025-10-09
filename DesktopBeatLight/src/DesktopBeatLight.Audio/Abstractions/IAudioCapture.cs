namespace DesktopBeatLight.Audio.Abstractions;

public interface IAudioCapture
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
    /// 获取当前音频数据的频谱信息
    /// </summary>
    /// <returns>频谱数据数组</returns>
   Task<float[]> GetSpectrumData();
}