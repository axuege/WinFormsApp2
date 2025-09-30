namespace DesktopBeatLight.Core.Abstractions;

using DesktopBeatLight.Core.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public interface IThemeConfigRepository
{
    /// <summary>
    /// ��ȡ������������
    /// </summary>
    /// <param name="cancellationToken">ȡ������</param>
    /// <returns>���������б�</returns>
    Task<List<ThemeConfig>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// ����ID��ȡ��������
    /// </summary>
    /// <param name="themeConfigId">��������ID</param>
    /// <param name="cancellationToken">ȡ������</param>
    /// <returns>�����������ã����δ�ҵ��򷵻� null</returns>
    Task<ThemeConfig?> GetByIdAsync(int themeConfigId, CancellationToken cancellationToken = default);

    /// <summary>
    /// �������������
    /// </summary>
    /// <param name="themeConfig">��������ʵ��</param>
    /// <param name="cancellationToken">ȡ������</param>
    /// <returns>��Ӻ���������ã��������ݿ����ɵ�ID��</returns>
    Task<ThemeConfig> AddAsync(ThemeConfig themeConfig, CancellationToken cancellationToken = default);

    /// <summary>
    /// ������������
    /// </summary>
    /// <param name="themeConfig">��������ʵ��</param>
    /// <param name="cancellationToken">ȡ������</param>
    /// <returns>�Ƿ���³ɹ�</returns>
    Task<bool> UpdateAsync(ThemeConfig themeConfig, CancellationToken cancellationToken = default);

    /// <summary>
    /// ɾ����������
    /// </summary>
    /// <param name="themeConfigId">��������ID</param>
    /// <param name="cancellationToken">ȡ������</param>
    /// <returns>�Ƿ�ɾ���ɹ�</returns>
    Task<bool> DeleteAsync(int themeConfigId, CancellationToken cancellationToken = default);

    /// <summary>
    /// ����Ĭ������
    /// </summary>
    /// <param name="themeConfigId">Ҫ����ΪĬ�ϵ�����ID</param>
    /// <param name="cancellationToken">ȡ������</param>
    /// <returns>�Ƿ����óɹ�</returns>
    Task<bool> SetDefaultAsync(int themeConfigId, CancellationToken cancellationToken = default);
}