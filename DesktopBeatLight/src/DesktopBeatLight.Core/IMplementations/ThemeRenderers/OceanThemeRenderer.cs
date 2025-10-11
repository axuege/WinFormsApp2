using DesktopBeatLight.Core.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using DesktopBeatLight.Core.Abstractions;
using System.Threading.Tasks;

namespace DesktopBeatLight.Core.IMplementations.ThemeRenderers;

/// <summary>
/// 海洋主题渲染器
/// 特点：清凉的蓝绿色调，包含海浪和泡沫效果
/// </summary>
public class OceanThemeRenderer : IThemeRenderer
{
    public async Task RenderAsync(ThemeRenderParams renderParams)
    {
        // 渲染操作通常需要在UI线程完成，这里使用Task.FromResult保持接口一致性
        // 如果有耗时的准备工作，可以在此处异步执行
        await Task.CompletedTask;
        RenderInternal(renderParams);
    }

    public Task InitializeAsync()
    {
        // 海洋主题无需特殊初始化
        return Task.CompletedTask;
    }

    [System.Obsolete("请使用RenderAsync方法")]
    public void Render(ThemeRenderParams renderParams)
    {
        RenderInternal(renderParams);
    }

    [System.Obsolete("请使用InitializeAsync方法")]
    public void Initialize()
    {
        // 海洋主题无需特殊初始化
    }

    /// <summary>
    /// 内部渲染实现方法
    /// </summary>
    private void RenderInternal(ThemeRenderParams renderParams)
    {
        if (renderParams.SpectrumData == null || renderParams.SpectrumData.Length == 0)
            return;

        var graphics = renderParams.Graphics;
        var themeConfig = renderParams.ThemeConfig;
        var spectrumData = renderParams.SpectrumData;
        var bounds = renderParams.Bounds;
        var isMuted = renderParams.IsMuted;

        Color primaryColor = ColorTranslator.FromHtml(themeConfig.PrimaryColor);
        Color gradientEndColor = ColorTranslator.FromHtml(themeConfig.GradientEndColor);
        Color accentColor = ColorTranslator.FromHtml(themeConfig.AccentColor);
        Color muteColor = ColorTranslator.FromHtml(themeConfig.MuteColor);

        // 如果静音，使用静音颜色
        if (isMuted)
        {
            primaryColor = Color.FromArgb(150, muteColor);
            accentColor = Color.FromArgb(100, muteColor);
        }
        else if (!string.IsNullOrEmpty(themeConfig.PrimaryColor))
        {
            primaryColor = ColorTranslator.FromHtml(themeConfig.PrimaryColor);
        }
        else
        {
            primaryColor = Color.Blue; // 默认蓝色
        }

        // 计算每个频谱条的宽度
        int barWidth = bounds.Width / spectrumData.Length;
        if (barWidth < 2) barWidth = 2;

        for (int i = 0; i < spectrumData.Length; i++)
        {
            // 根据频谱值计算条的高度，并添加一些随机性模拟海浪效果
            float intensity = spectrumData[i];
            if (intensity < 0.05f) intensity = 0.05f; // 最小强度

            // 海浪效果：相邻条之间有一些相关性
            if (i > 0 && i < spectrumData.Length - 1)
            {
                intensity = (intensity + spectrumData[i - 1] * 0.4f + spectrumData[i + 1] * 0.4f) / 1.8f;
            }

            int barHeight = (int)(bounds.Height * intensity);
            if (barHeight < 2) barHeight = 2;

            // 计算条的位置（从底部向上绘制）
            int x = i * barWidth;
            int y = bounds.Height - barHeight;

            // 创建海洋效果的渐变
            using (LinearGradientBrush brush = new LinearGradientBrush(
                new Point(x, y),
                new Point(x, bounds.Height),
                Color.LightBlue,  // 顶部浅蓝色
                primaryColor       // 底部深蓝色
            ))
            {
                // 绘制频谱条
                graphics.FillRectangle(brush, x, y, barWidth - 1, barHeight);
            }

            // 绘制波浪顶部的白色泡沫
            if (barHeight > 5 && !isMuted) // 静音时不显示泡沫效果
            {
                using (SolidBrush foamBrush = new SolidBrush(Color.FromArgb(180, Color.White)))
                {
                    int foamHeight = (int)(barHeight * 0.2);
                    if (foamHeight < 2) foamHeight = 2;
                    graphics.FillRectangle(foamBrush, x, y, barWidth - 1, foamHeight);
                }
            }
        }
    }
}