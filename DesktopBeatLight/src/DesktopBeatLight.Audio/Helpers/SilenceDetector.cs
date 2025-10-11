
namespace DesktopBeatLight.Audio.Helpers;

/// <summary>
/// 基于能量门限的静音检测器。
/// </summary>
public sealed class SilenceDetector
{
    /// <summary>
    /// 检测门限，单位 dBFS（负值）。
    /// </summary>
    private readonly double _thresholdDb;

    /// <summary>
    /// 避免 log(0) 的最小 RMS 值。
    /// </summary>
    private const double Epsilon = 1e-10;
    
    public SilenceDetector(double thresholdDb)
    {
        // 阈值必须是 0 或负值
        if (thresholdDb > 0)
            throw new ArgumentOutOfRangeException(nameof(thresholdDb), "阈值必须是 0 或负值（dBFS）。");
        _thresholdDb = thresholdDb;
    }

    /// <summary>
    /// 判断当前帧是否为静音。
    /// </summary>
    /// <param name="samples">单声道 32-bit float 样本，范围 [-1,1]。</param>
    /// <returns>true：静音；false：有声音。</returns>
    public bool IsSilent(float[] samples)
    {
        // 空样本视为静音
        if (samples == null || samples.Length == 0)
            return true;
        // 计算平方和
        double sum = 0;
        foreach (var s in samples)
        {
            sum += s * s;
        }
        // 计算均方根（RMS）
        double rms = Math.Sqrt(sum / samples.Length);
        // 转换为 dBFS
        double rmsDb = 20 * Math.Log10(Math.Max(rms, Epsilon));
        // 与阈值比较
        return rmsDb < _thresholdDb;
    }
}