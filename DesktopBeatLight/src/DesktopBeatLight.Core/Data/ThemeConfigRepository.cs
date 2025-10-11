using DesktopBeatLight.Core.Models;
using DesktopBeatLight.Core.Abstractions;
using DesktopBeatLight.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace DesktopBeatLight.Core.Data;

public class ThemeConfigRepository : IThemeConfigRepository
{
    // ����ע������ݿ�������
    private readonly AppDbContext _dbContext;

    public ThemeConfigRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    /// <summary>
    /// ��ȡ������������
    /// </summary>
    /// <param name="cancellationToken">ȡ������</param>
    /// <returns>���������б�</returns>
    public async Task<List<ThemeConfig>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // ���ȡ������
        cancellationToken.ThrowIfCancellationRequested();
        // �����ݿ����첽��ȡ������������
        return await _dbContext.ThemeConfigs.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// ����ID��ȡ��������
    /// </summary>
    /// <param name="themeConfigId">��������ID</param>
    /// <param name="cancellationToken">ȡ������</param>
    /// <returns>�����������ã����δ�ҵ��򷵻� null</returns>
   public async Task<ThemeConfig?> GetByIdAsync(int themeConfigId, CancellationToken cancellationToken = default)
    {
        //���ȡ������
        cancellationToken.ThrowIfCancellationRequested();
        //�����ݿ����첽��ȡָ��ID����������
        return await _dbContext.ThemeConfigs
            .FirstOrDefaultAsync(t => t.ThemeConfigId == themeConfigId, cancellationToken);
    }

    /// <summary>
    /// ��������������
    /// </summary>
    /// <param name="themeConfig">��������ʵ��</param>
    /// <param name="cancellationToken">ȡ������</param>
    /// <returns>���Ӻ���������ã��������ݿ����ɵ�ID��</returns>
    public async Task<ThemeConfig> AddAsync(ThemeConfig themeConfig, CancellationToken cancellationToken = default)
    {
        //���ȡ������
        cancellationToken.ThrowIfCancellationRequested();
        // �������ΪĬ�����⣬��ȡ������Ĭ��
        if (themeConfig.IsDefault)
        {
            // �������е�ǰ����ΪĬ�ϵ�����
            var others = await _dbContext.ThemeConfigs.Where(t => t.IsDefault).ToListAsync(cancellationToken);
            others.ForEach(t => t.IsDefault = false);
        }
        // �������������õ����ݿ�
        await _dbContext.ThemeConfigs.AddAsync(themeConfig, cancellationToken);
        // �������
        await _dbContext.SaveChangesAsync(cancellationToken);
        return themeConfig;
    }

    /// <summary>
    /// ������������
    /// </summary>
    /// <param name="themeConfig">��������ʵ��</param>
    /// <param name="cancellationToken">ȡ������</param>
    /// <returns>�Ƿ���³ɹ�</returns>
    public async Task<bool> UpdateAsync(ThemeConfig themeConfig, CancellationToken cancellationToken = default)
    {
        //���ȡ������
        cancellationToken.ThrowIfCancellationRequested();
        // ������������Ƿ����
        var exists = await _dbContext.ThemeConfigs
            .AnyAsync(t => t.ThemeConfigId == themeConfig.ThemeConfigId, cancellationToken);
        if (!exists) return false;
        // �������ΪĬ�����⣬��ȡ������Ĭ��
        if (themeConfig.IsDefault)
        {
            // �������е�ǰ����ΪĬ�ϵ�����
            var others = await _dbContext.ThemeConfigs.Where(t => t.IsDefault).ToListAsync(cancellationToken);
            others.ForEach(t => t.IsDefault = false);
        }
        // ������������
        _dbContext.Entry(themeConfig).State = EntityState.Modified;
        // �������
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    /// <summary>
    /// ɾ����������
    /// </summary>
    /// <param name="themeConfigId">��������ID</param>
    /// <param name="cancellationToken">ȡ������</param>
    /// <returns>�Ƿ�ɾ���ɹ�</returns>
    public async Task<bool> DeleteAsync(int themeConfigId, CancellationToken cancellationToken = default)
    {
        //���ȡ������
        cancellationToken.ThrowIfCancellationRequested();
        // ����Ҫɾ������������
        var themeConfig = await _dbContext.ThemeConfigs
            .FirstOrDefaultAsync(t => t.ThemeConfigId == themeConfigId, cancellationToken);
        if (themeConfig == null) return false;
        // ɾ����������
        _dbContext.ThemeConfigs.Remove(themeConfig);
        // �������
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;

    }

    /// <summary>
    /// ����Ĭ������
    /// </summary>
    /// <param name="themeConfigId">Ҫ����ΪĬ�ϵ�����ID</param>
    /// <param name="cancellationToken">ȡ������</param>
    /// <returns>�Ƿ����óɹ�</returns>
   public async Task<bool> SetDefaultAsync(int themeConfigId, CancellationToken cancellationToken = default)
    {
        //���ȡ������
        cancellationToken.ThrowIfCancellationRequested();
        // ����Ҫ����ΪĬ�ϵ���������
        var theme = await _dbContext.ThemeConfigs
            .FindAsync(new object[] { themeConfigId }, cancellationToken);
        // ���δ�ҵ������� false
        if (theme == null) return false;
        // ȡ�����������Ĭ������
        var others = await _dbContext.ThemeConfigs.Where(t => t.IsDefault).ToListAsync(cancellationToken);
        others.ForEach(t => t.IsDefault = false);
        // ����ָ������ΪĬ��
        theme.IsDefault = true;
        // �������
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}