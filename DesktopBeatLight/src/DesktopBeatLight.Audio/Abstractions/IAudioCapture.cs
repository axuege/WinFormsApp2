namespace DesktopBeatLight.Audio.Abstractions;

public interface IAudioCapture
{
    /// <summary>
    /// ��ʼ��Ƶ����
    /// </summary>
    void StartCapture();
    /// <summary>
    /// ֹͣ��Ƶ����
    /// </summary>
    void StopCapture();
    /// <summary>
    /// ��ȡ��ǰ��Ƶ���ݵ�Ƶ����Ϣ
    /// </summary>
    /// <returns>Ƶ����������</returns>
    float[] GetSpectrumData();
}