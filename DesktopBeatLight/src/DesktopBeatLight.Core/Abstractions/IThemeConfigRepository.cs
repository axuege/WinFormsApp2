namespace DesktopBeatLight.Core.Abstractions;
using DesktopBeatLight.Core.Models.Enums;
using DesktopBeatLight.Core.Models;
public interface IThemeConfigRepository
{
    //<<summary>
    //返回所有主题配置
    //</summary>
    //<param name="cancellationToken">取消令牌</param>
    //<returns>主题配置列表</returns>
    Task<List<ThemeConfig>> GetAllThemesAsync(CancellationToken cancellationToken=default);
    //<<summary>
    //根据ID返回主题配置
    //</summary>
    //<param name="cancellationToken">取消令牌</param>
    //<returns>主题配置列表</returns>
    Task<ThemeConfig?> GetThemeByIdAsync(int ThemeConfigId, CancellationToken cancellationToken = default);
    //<<summary>
    //添加新主题配置
    //</summary>
    //<param name="cancellationToken">取消令牌</param>
    //<returns>主题配置列表</returns>
    Task AddThemeAsync(ThemeConfig themeConfig, CancellationToken cancellationToken = default);
    //<<summary>
    //更新现有主题配置
    //</summary>
    //<param name="cancellationToken">取消令牌</param>
    //<returns>主题配置列表</returns>
    Task UpdateThemeAsync(ThemeConfig themeConfig, CancellationToken cancellationToken = default);
    //<<summary>
    //删除主题配置
    //</summary>
    //<param name="cancellationToken">取消令牌</param>
    //<returns>主题配置列表</returns>
    Task DeleteThemeAsync(int ThemeConfigId, CancellationToken cancellationToken = default);
    //<<summary>
    //设置默认主题
    //</summary>
    //<param name="cancellationToken">取消令牌</param>
    //<returns>主题配置列表</returns>
    Task <bool> SetDefaultThemeAsync(int ThemeConfigId, CancellationToken cancellationToken = default);
}