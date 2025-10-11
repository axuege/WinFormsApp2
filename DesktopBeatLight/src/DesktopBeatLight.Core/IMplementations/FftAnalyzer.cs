using System.Numerics;
using DesktopBeatLight.Core.Constants;
using MathNet.Numerics.IntegralTransforms;

namespace DesktopBeatLight.Core.IMplementations;

/// <summary>
/// FFT频谱分析器：将音频时域数据转换为降采样后的幅度谱数据
/// 核心流程：时域数据 → 复数数组 → FFT频域转换 → 幅度谱计算 → 64点降采样
/// </summary>
public sealed class FftAnalyzer
{
    // 私有字段：仅保留必要配置
    private readonly int _fftSize;
    private readonly int _usefulFftPointCount;
    private readonly int _targetSpectrumCount;
    private readonly Complex[] _fftBuffer; 
    private float[]? _previousSpectrum; // 用于频谱平滑

    /// <summary>
    /// 构造函数：初始化FFT配置和缓冲区
    /// </summary>
    public FftAnalyzer()
    {
        // 从FftConstants读取统一配置
        _fftSize = FftConstants.FftSize;
        _usefulFftPointCount = FftConstants.UsefulFftPointCount;
        _targetSpectrumCount = FftConstants.TargetSpectrumPointCount;

        // 初始化复数缓冲区（长度=FFT大小）
        _fftBuffer = new Complex[_fftSize];
        _previousSpectrum = new float[_targetSpectrumCount];
    }

    /// <summary>
    /// 核心方法：处理音频时域数据，输出64点幅度谱（异步版本）
    /// </summary>
    /// <param name="audioData">音频捕获模块输出的原始时域数据（float数组）</param>
    /// <returns>降采样后的64点幅度谱（float数组，值范围0~1，对应频谱强度）</returns>
    public Task<float[]> ProcessAudioDataAsync(float[] audioData)
    {
        // 使用Task.Run在后台线程执行FFT计算等CPU密集型操作
        return Task.Run(() =>
        {
            return ProcessAudioDataInternal(audioData);
        });
    }

    /// <summary>
    /// 核心方法：处理音频时域数据，输出64点幅度谱
    /// </summary>
    /// <param name="audioData">音频捕获模块输出的原始时域数据（float数组）</param>
    /// <returns>降采样后的64点幅度谱（float数组，值范围0~1，对应频谱强度）</returns>
    public float[] ProcessAudioData(float[] audioData)
    {
        return ProcessAudioDataInternal(audioData);
    }

    /// <summary>
    /// 内部实现方法，供同步和异步版本调用
    /// </summary>
    private float[] ProcessAudioDataInternal(float[] audioData)
    {
        // 输入校验：确保数据长度足够
        if (audioData == null || audioData.Length < _fftSize)
        {
            // 如果数据不足，返回空频谱
            return new float[_targetSpectrumCount];
        }

        // 步骤1：时域数据（float）→ MathNet复数数组
        for (int i = 0; i < _fftSize; i++)
        {
            _fftBuffer[i] = new Complex(audioData[i], 0); // 虚部设为0（时域实数信号）
        }

        // 步骤2：执行FFT（时域→频域）
        Fourier.Forward(_fftBuffer, FourierOptions.Default);

        // 步骤3：频域复数→幅度谱（仅保留前半段有效数据，丢弃对称部分）
        float[] fullAmplitudeSpectrum = new float[_usefulFftPointCount];
        for (int i = 0; i < _usefulFftPointCount; i++)
        {
            // 幅度=复数模长 ÷ FFT大小（归一化，确保输出范围0~1）
            fullAmplitudeSpectrum[i] = (float)(_fftBuffer[i].Magnitude / _fftSize);
        }

        // 步骤4：512点幅度谱→64点降采样（分块取最大值，突出频谱峰值）
        float[] downsampledSpectrum = DownsampleToTargetCount(fullAmplitudeSpectrum);

        // 步骤5：应用频谱平滑
        ApplySmoothing(downsampledSpectrum);

        return downsampledSpectrum;
    }

    /// <summary>
    /// 辅助方法：将512点幅度谱降采样为64点
    /// </summary>
    private float[] DownsampleToTargetCount(float[] fullSpectrum)
    {
        int step = fullSpectrum.Length / _targetSpectrumCount; // 分块步长：512/64=8
        float[] result = new float[_targetSpectrumCount];

        for (int i = 0; i < _targetSpectrumCount; i++)
        {
            float maxAmplitude = 0;
            // 遍历当前分块的8个点，取最大值（保证灯带能响应频谱峰值）
            for (int j = 0; j < step; j++)
            {
                int index = i * step + j;
                if (fullSpectrum[index] > maxAmplitude)
                {
                    maxAmplitude = fullSpectrum[index];
                }
            }
            result[i] = maxAmplitude;
        }

        return result;
    }

    /// <summary>
    /// 应用频谱平滑，避免视觉闪烁
    /// </summary>
    private void ApplySmoothing(float[] currentSpectrum)
    {
        if (_previousSpectrum == null) return;

        for (int i = 0; i < currentSpectrum.Length; i++)
        {
            // 当前值 = 新值 * (1-平滑系数) + 旧值 * 平滑系数
            currentSpectrum[i] = 
                currentSpectrum[i] * (1 - FftConstants.SpectrumSmoothingFactor) + 
                _previousSpectrum[i] * FftConstants.SpectrumSmoothingFactor;

            // 确保最小值
            if (currentSpectrum[i] < FftConstants.MinSpectrumValue)
            {
                currentSpectrum[i] = FftConstants.MinSpectrumValue;
            }
        }

        // 保存当前频谱用于下次平滑
        Array.Copy(currentSpectrum, _previousSpectrum, currentSpectrum.Length);
    }
}