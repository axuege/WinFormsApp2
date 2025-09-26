using DesktopBeatLight.Core.Models;
using DesktopBeatLight.Core.Models.Enums;
using DesktopBeatLight.Core.Abstractions;
namespace DesktopBeatLight.Core.IMplementations.ThemeRenderers;
public class ThemeConfigRepository : IThemeConfigRepository
{
    private readonly AppDbContext DbContex;


    //<<summary>
    //返回所有主题配置
    //</summary>
    //<param name="cancellationToken">取消令牌</param>
    //<returns>主题配置列表</returns>
    public async Task<List<ThemeConfig>> GetAllThemesAsync(CancellationToken cancellationToken = default)
    {
        //检查是否有取消请求，如果有则返回空列表
        if (cancellationToken.IsCancellationRequested)
        {
            return new List<ThemeConfig>();
        }
        //返回所有主题配置
        var thems=await DbContex.ThemeConfigs.ToListAsync();

    }
    //<<summary>
    //根据ID返回主题配置
    //</summary>
    //<param name="cancellationToken">取消令牌</param>
    //<returns>主题配置列表</returns>
    public async Task<ThemeConfig?> GetThemeByIdAsync(int ThemeConfigId, CancellationToken cancellationToken = default)
    {
        //根据ID拿到主题配置
        if (cancellationToken.IsCancellationRequested)
        {
            return null;
        }

    }
    //<<summary>
    //添加新主题配置
    //</summary>
    //<param name="cancellationToken">取消令牌</param>
    //<returns>主题配置列表</returns>
    public async Task AddThemeAsync(ThemeConfig themeConfig, CancellationToken cancellationToken = default)
    {
        //添加新主题配置
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }
    }
    //<<summary>
    //更新现有主题配置
    //</summary>
    //<param name="cancellationToken">取消令牌</param>
    //<returns>主题配置列表</returns>
    public async Task UpdateThemeAsync(ThemeConfig themeConfig, CancellationToken cancellationToken = default)
    {
        //更新现有主题配置
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }
    }
    //<<summary>
    //删除主题配置
    //</summary>
    //<param name="cancellationToken">取消令牌</param>
    //<returns>主题配置列表</returns>
    public async Task DeleteThemeAsync(int ThemeConfigId, CancellationToken cancellationToken = default)
    {
        //删除主题配置
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }
    }
    //<<summary>
    //设置默认主题
    //</summary>
    //<param name="cancellationToken">取消令牌</param>
    //<returns>主题配置列表</returns>
    public async Task <bool> SetDefaultThemeAsync(int ThemeConfigId, CancellationToken cancellationToken = default)
    {
        //设置默认主题
        if (cancellationToken.IsCancellationRequested)
        {
            return false;
        }
    }
}