using DesktopBeatLight.Core.Models;
using DesktopBeatLight.Core.Models.Enums;
using DesktopBeatLight.Core.Abstractions;
namespace DesktopBeatLight.Core.IMplementations.ThemeRenderers;
public class ThemeConfigRepository : IThemeConfigRepository
{
    private readonly AppDbContext DbContex;


    //<<summary>
    //����������������
    //</summary>
    //<param name="cancellationToken">ȡ������</param>
    //<returns>���������б�</returns>
    public async Task<List<ThemeConfig>> GetAllThemesAsync(CancellationToken cancellationToken = default)
    {
        //����Ƿ���ȡ������������򷵻ؿ��б�
        if (cancellationToken.IsCancellationRequested)
        {
            return new List<ThemeConfig>();
        }
        //����������������
        var thems=await DbContex.ThemeConfigs.ToListAsync();

    }
    //<<summary>
    //����ID������������
    //</summary>
    //<param name="cancellationToken">ȡ������</param>
    //<returns>���������б�</returns>
    public async Task<ThemeConfig?> GetThemeByIdAsync(int ThemeConfigId, CancellationToken cancellationToken = default)
    {
        //����ID�õ���������
        if (cancellationToken.IsCancellationRequested)
        {
            return null;
        }

    }
    //<<summary>
    //�������������
    //</summary>
    //<param name="cancellationToken">ȡ������</param>
    //<returns>���������б�</returns>
    public async Task AddThemeAsync(ThemeConfig themeConfig, CancellationToken cancellationToken = default)
    {
        //�������������
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }
    }
    //<<summary>
    //����������������
    //</summary>
    //<param name="cancellationToken">ȡ������</param>
    //<returns>���������б�</returns>
    public async Task UpdateThemeAsync(ThemeConfig themeConfig, CancellationToken cancellationToken = default)
    {
        //����������������
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }
    }
    //<<summary>
    //ɾ����������
    //</summary>
    //<param name="cancellationToken">ȡ������</param>
    //<returns>���������б�</returns>
    public async Task DeleteThemeAsync(int ThemeConfigId, CancellationToken cancellationToken = default)
    {
        //ɾ����������
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }
    }
    //<<summary>
    //����Ĭ������
    //</summary>
    //<param name="cancellationToken">ȡ������</param>
    //<returns>���������б�</returns>
    public async Task <bool> SetDefaultThemeAsync(int ThemeConfigId, CancellationToken cancellationToken = default)
    {
        //����Ĭ������
        if (cancellationToken.IsCancellationRequested)
        {
            return false;
        }
    }
}