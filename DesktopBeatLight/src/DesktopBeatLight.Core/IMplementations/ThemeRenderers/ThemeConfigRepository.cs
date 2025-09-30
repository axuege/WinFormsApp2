using DesktopBeatLight.Core.Models;
using DesktopBeatLight.Core.Models.Enums;
using DesktopBeatLight.Core.Abstractions;
using DesktopBeatLight.Core.Data;
using Microsoft.EntityFrameworkCore;
namespace DesktopBeatLight.Core.IMplementations.ThemeRenderers;

public class ThemeConfigRepository : IThemeConfigRepository
{
    // 依赖注入的数据库上下文
    private readonly AppDbContext _dbContext;

    public ThemeConfigRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    /// <summary>
    /// 获取所有主题配置
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>主题配置列表</returns>
    public async Task<List<ThemeConfig>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // 检查取消令牌
        cancellationToken.ThrowIfCancellationRequested();
        // 从数据库中异步获取所有主题配置
        return await _dbContext.ThemeConfigs.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 根据ID获取主题配置
    /// </summary>
    /// <param name="themeConfigId">主题配置ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>单个主题配置，如果未找到则返回 null</returns>
   public async Task<ThemeConfig?> GetByIdAsync(int themeConfigId, CancellationToken cancellationToken = default)
    {
        //检查取消令牌
        cancellationToken.ThrowIfCancellationRequested();
        //从数据库中异步获取指定ID的主题配置
        return await _dbContext.ThemeConfigs
            .FirstOrDefaultAsync(t => t.ThemeConfigId == themeConfigId, cancellationToken);
    }

    /// <summary>
    /// 添加新主题配置
    /// </summary>
    /// <param name="themeConfig">主题配置实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>添加后的主题配置（包含数据库生成的ID）</returns>
    public async Task<ThemeConfig> AddAsync(ThemeConfig themeConfig, CancellationToken cancellationToken = default)
    {
        //检查取消令牌
        cancellationToken.ThrowIfCancellationRequested();
        // 如果设置为默认主题，先取消其他默认
        if (themeConfig.IsDefault)
        {
            // 查找所有当前设置为默认的主题
            var others = await _dbContext.ThemeConfigs.Where(t => t.IsDefault).ToListAsync(cancellationToken);
            others.ForEach(t => t.IsDefault = false);
        }
        // 添加新主题配置到数据库
        await _dbContext.ThemeConfigs.AddAsync(themeConfig, cancellationToken);
        // 保存更改
        await _dbContext.SaveChangesAsync(cancellationToken);
        return themeConfig;
    }

    /// <summary>
    /// 更新主题配置
    /// </summary>
    /// <param name="themeConfig">主题配置实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否更新成功</returns>
    public async Task<bool> UpdateAsync(ThemeConfig themeConfig, CancellationToken cancellationToken = default)
    {
        //检查取消令牌
        cancellationToken.ThrowIfCancellationRequested();
        // 检查主题配置是否存在
        var exists = await _dbContext.ThemeConfigs
            .AnyAsync(t => t.ThemeConfigId == themeConfig.ThemeConfigId, cancellationToken);
        if (!exists) return false;
        // 如果设置为默认主题，先取消其他默认
        if (themeConfig.IsDefault)
        {
            // 查找所有当前设置为默认的主题
            var others = await _dbContext.ThemeConfigs.Where(t => t.IsDefault).ToListAsync(cancellationToken);
            others.ForEach(t => t.IsDefault = false);
        }
        // 更新主题配置
        _dbContext.Entry(themeConfig).State = EntityState.Modified;
        // 保存更改
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    /// <summary>
    /// 删除主题配置
    /// </summary>
    /// <param name="themeConfigId">主题配置ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否删除成功</returns>
    public async Task<bool> DeleteAsync(int themeConfigId, CancellationToken cancellationToken = default)
    {
        //检查取消令牌
        cancellationToken.ThrowIfCancellationRequested();
        // 查找要删除的主题配置
        var themeConfig = await _dbContext.ThemeConfigs
            .FirstOrDefaultAsync(t => t.ThemeConfigId == themeConfigId, cancellationToken);
        if (themeConfig == null) return false;
        // 删除主题配置
        _dbContext.ThemeConfigs.Remove(themeConfig);
        // 保存更改
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;

    }

    /// <summary>
    /// 设置默认主题
    /// </summary>
    /// <param name="themeConfigId">要设置为默认的主题ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否设置成功</returns>
   public async Task<bool> SetDefaultAsync(int themeConfigId, CancellationToken cancellationToken = default)
    {
        //检查取消令牌
        cancellationToken.ThrowIfCancellationRequested();
        // 查找要设置为默认的主题配置
        var theme = await _dbContext.ThemeConfigs
            .FindAsync(new object[] { themeConfigId }, cancellationToken);
        // 如果未找到，返回 false
        if (theme == null) return false;
        // 取消其他主题的默认设置
        var others = await _dbContext.ThemeConfigs.Where(t => t.IsDefault).ToListAsync(cancellationToken);
        others.ForEach(t => t.IsDefault = false);
        // 设置指定主题为默认
        theme.IsDefault = true;
        // 保存更改
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}