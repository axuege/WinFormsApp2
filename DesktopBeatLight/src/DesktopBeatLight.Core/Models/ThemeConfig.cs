//���������ռ�
using DesktopBeatLight.Core.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//�����ռ�: DesktopBeatLight.Core.Models
namespace DesktopBeatLight.Core.Models;
// ����������
public class ThemeConfig
{
    //����
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // ����ID
    // 1. ���������ʶ�������ֶΣ�
    /// <summary>
    /// ����Ψһ��ʶ�����������ù̶�ID���û��Զ�������������
    /// </summary>
    public int ThemeConfigId { get; set; }

    /// <summary>
    /// �������ƣ���"�����"��"���"��
    /// </summary>
    // �ǿ�
    [Required]
    //���Ƴ���
    [MaxLength(50)] 
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// �Ƿ�ΪϵͳĬ�����⣨��һ��Ĭ�����⣬���ڳ�ʼ����
    /// </summary>
    public bool IsDefault { get; set; } = false;


    // 2. ������ɫ���ã���Զ�̬��Ⱦ�����Ǿ�̬������
    /// <summary>
    /// ��ɫ�����ƴ�����ɫ�����������ĺ�ɫ��
    /// </summary>
    public string PrimaryColor { get; set; } = "#FF4500"; // Ĭ�ϳ�ɫ��ʾ����

    /// <summary>
    /// �����յ�ɫ������ɫ���γɽ��䣬���������Ļ�ɫ��
    /// </summary>
    public string GradientEndColor { get; set; } = "#FFFF00"; // Ĭ�ϻ�ɫ��ʾ����

    /// <summary>
    /// ǿ��ɫ����ֵ����ɫ���������İ�ɫ��
    /// </summary>
    public string AccentColor { get; set; } = "#FFFFFF"; // Ĭ�ϰ�ɫ��ʾ����

    /// <summary>
    /// ����״̬��ɫ������Ƶʱ�Ļ���ɫ��ͨ��Ϊ��ɫ��
    /// </summary>
    public string MuteColor { get; set; } = "#333333"; // Ĭ����ң�ʾ����


    // 3. �ƴ��������
    /// <summary>
    /// �ƴ��߶ȣ����أ���Χ 4-32��Ĭ��8��
    /// </summary>
    public int LightHeight { get; set; } = 8;

    /// <summary>
    /// �ƴ�λ�ã�ö�٣�Bottom/Top/Left/Right��Ĭ�ϵײ���
    /// </summary>
    public LightPosition LightPosition { get; set; } = LightPosition.Bottom;

    /// <summary>
    /// ȫ�����ȣ�0-100��Ĭ��80��
    /// </summary>
    public int Brightness { get; set; } = 80;

    /// <summary>
    /// ����ʱ�����ȣ�0-100��Ĭ��20��ͨ�������������ȣ�
    /// </summary>
    public int MuteBrightness { get; set; } = 20;


    // 4. ��Ϊ����
    /// <summary>
    /// ����ʱ�Ƿ���ͣ��Ⱦ��true=����ʾ��̬����ɫ��false=������Ӧ��������
    /// </summary>
    public bool PauseOnMute { get; set; } = true;
    //�޲ι���
    public ThemeConfig()
    {

    }
    //ȫ�ι���
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
