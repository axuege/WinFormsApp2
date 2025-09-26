namespace DesktopBeatLight.Core.Abstractions;
using DesktopBeatLight.Core.Models.Enums;
using DesktopBeatLight.Core.Models;
public interface IThemeConfigRepository
{
    //<<summary>
    //����������������
    //</summary>
    //<param name="cancellationToken">ȡ������</param>
    //<returns>���������б�</returns>
    Task<List<ThemeConfig>> GetAllThemesAsync(CancellationToken cancellationToken=default);
    //<<summary>
    //����ID������������
    //</summary>
    //<param name="cancellationToken">ȡ������</param>
    //<returns>���������б�</returns>
    Task<ThemeConfig?> GetThemeByIdAsync(int ThemeConfigId, CancellationToken cancellationToken = default);
    //<<summary>
    //�������������
    //</summary>
    //<param name="cancellationToken">ȡ������</param>
    //<returns>���������б�</returns>
    Task AddThemeAsync(ThemeConfig themeConfig, CancellationToken cancellationToken = default);
    //<<summary>
    //����������������
    //</summary>
    //<param name="cancellationToken">ȡ������</param>
    //<returns>���������б�</returns>
    Task UpdateThemeAsync(ThemeConfig themeConfig, CancellationToken cancellationToken = default);
    //<<summary>
    //ɾ����������
    //</summary>
    //<param name="cancellationToken">ȡ������</param>
    //<returns>���������б�</returns>
    Task DeleteThemeAsync(int ThemeConfigId, CancellationToken cancellationToken = default);
    //<<summary>
    //����Ĭ������
    //</summary>
    //<param name="cancellationToken">ȡ������</param>
    //<returns>���������б�</returns>
    Task <bool> SetDefaultThemeAsync(int ThemeConfigId, CancellationToken cancellationToken = default);
}