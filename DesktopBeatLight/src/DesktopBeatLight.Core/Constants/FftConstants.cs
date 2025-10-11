namespace DesktopBeatLight.Core.Constants;

/// <summary>
/// FFT相关常量定义
/// 包含FFT分析所需的所有配置参数
/// </summary>
public static class FftConstants
{
    /// <summary>
    /// FFT大小（采样点数）
    /// 必须为2的幂次，值越大频率分辨率越高，但计算成本也越高
    /// </summary>
    public const int FftSize = 1024;
    
    /// <summary>
    /// 有效的FFT点数
    /// 通常为FftSize的一半（实信号FFT的频谱是对称的）
    /// </summary>
    public const int UsefulFftPointCount = 512;
    
    /// <summary>
    /// 目标频谱点数量（降采样后的点数）
    /// 决定了可视化条带的数量
    /// </summary>
    public const int TargetSpectrumPointCount = 64;
    
    /// <summary>
    /// 频谱平滑系数（0-1）
    /// 值越大，频谱变化越平滑，但响应速度越慢
    /// </summary>
    public const float SpectrumSmoothingFactor = 0.3f;
    
    /// <summary>
    /// 最小频谱值（低于此值的频谱被视为静音）
    /// </summary>
    public const float MinSpectrumValue = 0.05f;
}