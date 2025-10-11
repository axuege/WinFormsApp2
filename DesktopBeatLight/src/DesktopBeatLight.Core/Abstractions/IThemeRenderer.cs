using DesktopBeatLight.Core.Models;
using System.Drawing;
using System.Threading.Tasks;

namespace DesktopBeatLight.Core.Abstractions;

/// <summary>
/// 主题渲染器接口
/// 定义了渲染音频可视化效果的核心方法
/// </summary>
public interface IThemeRenderer
{
    /// <summary>
    /// 渲染音频可视化效果（异步版本）
    /// </summary>
    /// <param name="renderParams">渲染参数</param>
    Task RenderAsync(ThemeRenderParams renderParams);

    /// <summary>
    /// 初始化渲染器（异步版本）
    /// </summary>
    Task InitializeAsync();

    /// <summary>
    /// 渲染音频可视化效果
    /// </summary>
    /// <param name="renderParams">渲染参数</param>
    [System.Obsolete("请使用RenderAsync方法")]
    void Render(ThemeRenderParams renderParams);

    /// <summary>
    /// 初始化渲染器
    /// </summary>
    [System.Obsolete("请使用InitializeAsync方法")]
    void Initialize();
}