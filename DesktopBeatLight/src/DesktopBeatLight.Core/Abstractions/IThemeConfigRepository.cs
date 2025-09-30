namespace DesktopBeatLight.Core.Abstractions;

using DesktopBeatLight.Core.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public interface IThemeConfigRepository
{
    /// <summary>
    /// 获取所有主题配置
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>主题配置列表</returns>
    Task<List<ThemeConfig>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据ID获取主题配置
    /// </summary>
    /// <param name="themeConfigId">主题配置ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>单个主题配置，如果未找到则返回 null</returns>
    Task<ThemeConfig?> GetByIdAsync(int themeConfigId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 添加新主题配置
    /// </summary>
    /// <param name="themeConfig">主题配置实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>添加后的主题配置（包含数据库生成的ID）</returns>
    Task<ThemeConfig> AddAsync(ThemeConfig themeConfig, CancellationToken cancellationToken = default);

    /// <summary>
    /// 更新主题配置
    /// </summary>
    /// <param name="themeConfig">主题配置实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否更新成功</returns>
    Task<bool> UpdateAsync(ThemeConfig themeConfig, CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除主题配置
    /// </summary>
    /// <param name="themeConfigId">主题配置ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否删除成功</returns>
    Task<bool> DeleteAsync(int themeConfigId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 设置默认主题
    /// </summary>
    /// <param name="themeConfigId">要设置为默认的主题ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否设置成功</returns>
    Task<bool> SetDefaultAsync(int themeConfigId, CancellationToken cancellationToken = default);
}