//引用命名空间
using DesktopBeatLight.Core.Models.Enums;
//命名空间: DesktopBeatLight.Core.Models
namespace DesktopBeatLight.Core.Models;
// 主题配置类
public class ThemeConfig
{
    // 1. 主题基础标识（核心字段）
    /// <summary>
    /// 主题唯一标识（内置主题用固定ID，用户自定义主题自增）
    /// </summary>
    public int ThemeConfigId { get; set; }

    /// <summary>
    /// 主题名称（如"火焰红"、"深海蓝"）
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 是否为系统默认主题（仅一个默认主题，用于初始化）
    /// </summary>
    public bool IsDefault { get; set; } = false;


    // 2. 主题颜色配置（针对动态渲染，而非静态背景）
    /// <summary>
    /// 主色调（灯带基础色，如火焰主题的红色）
    /// </summary>
    public string PrimaryColor { get; set; } = "#FF4500"; // 默认橙色（示例）

    /// <summary>
    /// 渐变终点色（与主色调形成渐变，如火焰主题的黄色）
    /// </summary>
    public string GradientEndColor { get; set; } = "#FFFF00"; // 默认黄色（示例）

    /// <summary>
    /// 强调色（峰值高亮色，如最亮的白色）
    /// </summary>
    public string AccentColor { get; set; } = "#FFFFFF"; // 默认白色（示例）

    /// <summary>
    /// 静音状态颜色（无音频时的基础色，通常为暗色）
    /// </summary>
    public string MuteColor { get; set; } = "#333333"; // 默认深灰（示例）


    // 3. 灯带外观配置
    /// <summary>
    /// 灯带高度（像素，范围 4-32，默认8）
    /// </summary>
    public int LightHeight { get; set; } = 8;

    /// <summary>
    /// 灯带位置（枚举：Bottom/Top/Left/Right，默认底部）
    /// </summary>
    public LightPosition LightPosition { get; set; } = LightPosition.Bottom;

    /// <summary>
    /// 全局亮度（0-100，默认80）
    /// </summary>
    public int Brightness { get; set; } = 80;

    /// <summary>
    /// 静音时的亮度（0-100，默认20，通常低于正常亮度）
    /// </summary>
    public int MuteBrightness { get; set; } = 20;


    // 4. 行为配置
    /// <summary>
    /// 静音时是否暂停渲染（true=仅显示静态静音色，false=继续响应但弱化）
    /// </summary>
    public bool PauseOnMute { get; set; } = true;
    //无参构造
    public ThemeConfig()
    {

    }
    //全参构造
    public ThemeConfig(int themeConfigId, string name, bool isDefault, string primaryColor, string gradientEndColor, string accentColor, string muteColor, int lightHeight, LightPosition lightPosition, int brightness, int muteBrightness, bool pauseOnMute)
    {
        ThemeConfigId = themeConfigId;
        Name = name;
        IsDefault = isDefault;
        PrimaryColor = primaryColor;
        GradientEndColor = gradientEndColor;
        AccentColor = accentColor;
        MuteColor = muteColor;
        LightHeight = lightHeight;
        LightPosition = lightPosition;
        Brightness = brightness;
        MuteBrightness = muteBrightness;
        PauseOnMute = pauseOnMute;
    }
}
