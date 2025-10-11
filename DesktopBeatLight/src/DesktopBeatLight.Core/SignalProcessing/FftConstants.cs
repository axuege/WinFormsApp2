namespace DesktopBeatLight.Core.SinalProcessing;

public static class FftConstants
{
    // 1. FFT算法基础配置（影响分析精度和性能）
    /// <summary>
    /// FFT计算的采样点数量（必须是2的幂，如256、512、1024）
    /// 选择1024：兼顾分析精度（能覆盖20Hz-20kHz音频范围）和性能（低CPU占用）
    /// </summary>
    public const int fftSize = 1024; //FFT大小

    /// <summary>
    /// 音频采样率（与WASAPI捕获的采样率保持一致，默认44100Hz，音频标准采样率）
    /// 若后续修改WASAPI采样率，需同步更新此值
    /// </summary>
    public const int sampleRate = 44100; //采样率
    /// <summary>
    /// FFT结果的有效数据长度（FFT结果对称，仅前半段有效）
    /// 计算公式：FftSize / 2（1024/2=512个有效点）
    /// </summary>
    public static readonly int fftResultLength = fftSize / 2;

    // 2. 频谱处理配置（影响灯带渲染效果）
    /// <summary>
    /// 最终输出给灯带的频谱频点数量（降采样后的目标点数）
    /// 选择64：平衡视觉细节（不会太稀疏）和渲染性能（不会太密集）
    /// 对应之前FftAnalyzer的DownsampleTo64Points方法
    /// </summary>
    public const int TargetSpectrumPointCount = 64;

    /// <summary>
    /// 频谱幅度放大系数（调整灯带柱形的高度灵敏度）
    /// 5倍放大：避免原始频谱值过小导致灯带无明显变化（对应火焰主题的heightRatio计算）
    /// 若觉得灯带太矮/太高，可调整此值（如3f、7f）
    /// </summary>
    public const float SpectrumAmplificationFactor = 5f;

    /// <summary>
    /// 频谱幅度的最大限制（避免极端值导致灯带超出窗口范围）
    /// 1f对应100%窗口高度，与渲染时的Math.Min(..., 1f)配合使用
    /// </summary>
    public const float MaxSpectrumAmplitude = 1f;

    // 关联模块配置（与静音检测、渲染定时器联动）
    /// <summary>
    /// 渲染帧率对应的帧间隔（毫秒）
    /// 50FPS = 1000ms / 50 = 20ms，与之前PeriodicTimer的20ms间隔一致
    /// </summary>
    public const int RenderFrameIntervalMs = 20;

    /// <summary>
    /// 静音检测的连续帧阈值（触发静音的帧数）
    /// 计算逻辑：2秒 / 20ms每帧 = 100帧，与之前SilenceDetector的100帧判断一致
    /// </summary>
    public const int SilenceTriggerFrameCount = 100;

    /// <summary>
    /// DPI适配的基准值（Windows默认DPI为96）
    /// 用于DpiHelper计算控件缩放比例（如ScaleByDpi方法）
    /// </summary>
    public const int BaseDpi = 96;
    
}