using DesktopBeatLight.Core.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using DesktopBeatLight.Core.Abstractions;
using System.Threading.Tasks;

namespace DesktopBeatLight.Core.IMplementations.ThemeRenderers;

/// <summary>
/// 霓虹灯主题渲染器
/// 特点：鲜艳的霓虹色调，包含发光和闪烁效果
/// </summary>
public class NeonThemeRenderer : IThemeRenderer
{
    private int _frameCount = 0;

    public async Task RenderAsync(ThemeRenderParams renderParams)
    {
        // 渲染操作通常需要在UI线程完成，这里使用Task.FromResult保持接口一致性
        // 如果有耗时的准备工作，可以在此处异步执行
        await Task.CompletedTask;
        RenderInternal(renderParams);
    }

    public Task InitializeAsync()
    {
        _frameCount = 0;
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
        _frameCount = 0;
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

        // 如果静音，使用静音颜色
        if (isMuted) {
            Color muteColor = ColorTranslator.FromHtml(themeConfig.MuteColor);
            using (SolidBrush muteBrush = new SolidBrush(Color.FromArgb(100, muteColor))) {
                // 绘制简单的静音状态提示条
                int muteBarWidth = bounds.Width / 10;
                for (int i = 0; i < 10; i++)
                {
                    int x = i * (muteBarWidth + 5) + (bounds.Width - 10 * (muteBarWidth + 5) + 5) / 2;
                    int height = (_frameCount / 20 + i) % 3 + 1;
                    int barHeight = (int)(bounds.Height * 0.2 * height);
                    int y = bounds.Height - barHeight;
                    graphics.FillRectangle(muteBrush, x, y, muteBarWidth - 1, barHeight);
                }
            }
            return;
        }

        Color primaryColor = ColorTranslator.FromHtml(themeConfig.PrimaryColor);
        Color gradientEndColor = ColorTranslator.FromHtml(themeConfig.GradientEndColor);
        Color accentColor = ColorTranslator.FromHtml(themeConfig.AccentColor);

        // 计算每个频谱条的宽度
        int barWidth = bounds.Width / spectrumData.Length;
        if (barWidth < 3) barWidth = 3; // 霓虹灯效果需要稍宽的条

        // 更新帧计数器
        _frameCount++;
        if (_frameCount > 1000) _frameCount = 0;

        for (int i = 0; i < spectrumData.Length; i++)
        {
            // 根据频谱值计算条的高度
            float intensity = spectrumData[i];
            if (intensity < 0.1f) intensity = 0.1f; // 霓虹灯效果需要更高的最小强度

            int barHeight = (int)(bounds.Height * intensity);
            if (barHeight < 5) barHeight = 5;

            // 计算条的位置（从底部向上绘制）
            int x = i * barWidth;
            int y = bounds.Height - barHeight;

            // 霓虹灯闪烁效果：周期性地增加亮度
            float flashIntensity = 1.0f;
            if ((i + _frameCount) % 30 < 5) // 每30帧闪烁一次，持续5帧
            {
                flashIntensity = 1.3f;
            }

            // 创建霓虹灯效果的发光区域（光晕效果）
            using (GraphicsPath glowPath = new GraphicsPath())
            {
                glowPath.AddRectangle(new Rectangle(x - 2, y - 2, barWidth + 4, barHeight + 4));
                using (PathGradientBrush glowBrush = new PathGradientBrush(glowPath))
                {
                    glowBrush.CenterColor = Color.FromArgb(100, Color.White);
                    glowBrush.SurroundColors = new Color[] { Color.Transparent };
                    graphics.FillPath(glowBrush, glowPath);
                }
            }

            // 选择霓虹灯颜色（根据位置变化）
            Color neonColor = GetNeonColor(i, spectrumData.Length, primaryColor, accentColor, flashIntensity);

            // 绘制霓虹灯主条
            using (SolidBrush neonBrush = new SolidBrush(neonColor))
            {
                graphics.FillRectangle(neonBrush, x, y, barWidth - 1, barHeight);
            }

            // 绘制霓虹灯顶部的亮点
            using (SolidBrush highlightBrush = new SolidBrush(Color.White))
            {
                graphics.FillRectangle(highlightBrush, x + 2, y, barWidth - 5, 2);
            }
        }
    }

    // 根据位置选择霓虹灯颜色
    private Color GetNeonColor(int index, int totalCount, Color primary, Color accent, float intensity)
    {
        // 创建颜色渐变
        float hue1 = ColorToHue(primary);
        float hue2 = ColorToHue(accent);
        float hueDiff = (hue2 - hue1 + 360) % 360;
        float hue = (hue1 + (hueDiff * index / totalCount)) % 360;

        // 转换回RGB颜色
        return HueToColor(hue, 0.9f, 0.9f * intensity);
    }

    // RGB颜色转HSL色相
    private float ColorToHue(Color color)
    {
        float r = color.R / 255f;
        float g = color.G / 255f;
        float b = color.B / 255f;

        float max = Math.Max(r, Math.Max(g, b));
        float min = Math.Min(r, Math.Min(g, b));
        float delta = max - min;

        if (delta == 0) return 0;

        float hue = 0;
        if (max == r) hue = ((g - b) / delta) % 6;
        else if (max == g) hue = (b - r) / delta + 2;
        else hue = (r - g) / delta + 4;

        hue *= 60;
        if (hue < 0) hue += 360;

        return hue;
    }

    // HSL色相转RGB颜色
    private Color HueToColor(float hue, float saturation, float lightness)
    {
        float c = (1 - Math.Abs(2 * lightness - 1)) * saturation;
        float x = c * (1 - Math.Abs((hue / 60) % 2 - 1));
        float m = lightness - c / 2;

        float r = 0, g = 0, b = 0;
        if (hue >= 0 && hue < 60) { r = c; g = x; }
        else if (hue < 120) { r = x; g = c; }
        else if (hue < 180) { g = c; b = x; }
        else if (hue < 240) { g = x; b = c; }
        else if (hue < 300) { r = x; b = c; }
        else { r = c; b = x; }

        int rByte = (int)((r + m) * 255);
        int gByte = (int)((g + m) * 255);
        int bByte = (int)((b + m) * 255);

        return Color.FromArgb(
            Math.Max(0, Math.Min(255, rByte)),
            Math.Max(0, Math.Min(255, gByte)),
            Math.Max(0, Math.Min(255, bByte))
        );
    }
}