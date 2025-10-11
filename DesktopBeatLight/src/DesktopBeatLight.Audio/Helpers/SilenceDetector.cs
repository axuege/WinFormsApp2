
namespace DesktopBeatLight.Audio.Helpers;

/// <summary>
/// �����������޵ľ����������
/// </summary>
public sealed class SilenceDetector
{
    /// <summary>
    /// ������ޣ���λ dBFS����ֵ����
    /// </summary>
    private readonly double _thresholdDb;

    /// <summary>
    /// ���� log(0) ����С RMS ֵ��
    /// </summary>
    private const double Epsilon = 1e-10;
    
    public SilenceDetector(double thresholdDb)
    {
        // ��ֵ������ 0 ��ֵ
        if (thresholdDb > 0)
            throw new ArgumentOutOfRangeException(nameof(thresholdDb), "��ֵ������ 0 ��ֵ��dBFS����");
        _thresholdDb = thresholdDb;
    }

    /// <summary>
    /// �жϵ�ǰ֡�Ƿ�Ϊ������
    /// </summary>
    /// <param name="samples">������ 32-bit float ��������Χ [-1,1]��</param>
    /// <returns>true��������false����������</returns>
    public bool IsSilent(float[] samples)
    {
        // ��������Ϊ����
        if (samples == null || samples.Length == 0)
            return true;
        // ����ƽ����
        double sum = 0;
        foreach (var s in samples)
        {
            sum += s * s;
        }
        // �����������RMS��
        double rms = Math.Sqrt(sum / samples.Length);
        // ת��Ϊ dBFS
        double rmsDb = 20 * Math.Log10(Math.Max(rms, Epsilon));
        // ����ֵ�Ƚ�
        return rmsDb < _thresholdDb;
    }
}