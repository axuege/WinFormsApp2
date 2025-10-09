namespace DesktopBeatLight.Audio.Abstractions;

public interface IAudioCapture
{
    /// <summary>
    /// ��ʼ��Ƶ����
    /// </summary>
    Task StartCapture();
    /// <summary>
    /// ֹͣ��Ƶ����
    /// </summary>
    Task StopCapture();
    /// <summary>
    /// ��ȡ��ǰ��Ƶ���ݵ�Ƶ����Ϣ
    /// </summary>
    /// <returns>Ƶ����������</returns>
   Task<float[]> GetSpectrumData();
}