using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows.Forms;
using DesktopBeatLight.Core.Data;
using DesktopBeatLight.Core.Models;
using DesktopBeatLight.Core.Models.Enums;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace DesktopBeatLight.UI;

public partial class Form1 : Form
{
    private readonly AppDbContext _dbContext;

    public Form1(AppDbContext dbcontent)
    {
        
        InitializeComponent();
        _dbContext = dbcontent;
        // �ڴ������ʱִ�в���
        this.Load += Form1_Load;
    }
 

private void DrawGradientBackground()
{
    // �������Ǵ����ݿ��ȡ�����������ã�������ģ��ʹ��֮ǰ���������������ɫ
    Color primaryColor = ColorTranslator.FromHtml("#FF69B4");
    Color gradientEndColor = ColorTranslator.FromHtml("#FF1493");

    using (LinearGradientBrush brush = new LinearGradientBrush(
        this.ClientRectangle,
        primaryColor,
        gradientEndColor,
        LinearGradientMode.Vertical))
    {
        using (Graphics g = this.CreateGraphics())
        {
            g.FillRectangle(brush, this.ClientRectangle);
        }
    }
}
private async void Form1_Load(object sender, EventArgs e)
    {
        try
        {
            var canConnect = await _dbContext.Database.CanConnectAsync();
            MessageBox.Show($"���ݿ����Ӳ���: {(canConnect ? "�ɹ�" : "ʧ��")}");

            if (canConnect)
            {
                var themes =  _dbContext.ThemeConfigs.ToList();

                if (themes.Count == 0)
                {
                    MessageBox.Show("ThemeConfigs ��Ϊ�գ����һ�����ۻ����������ݡ�");

                    var newTheme = new ThemeConfig
                    {
                        ThemeConfigId=2,
                        Name = "���ۻ�������",
                        IsDefault = false,
                        PrimaryColor = "#FF69B4",
                        GradientEndColor = "#FF1493",
                        AccentColor = "#FF00FF",
                        MuteColor = "#FFC0CB",
                        LightHeight = 10,
                        LightPosition = LightPosition.Bottom,
                        Brightness = 95,
                        MuteBrightness = 35,
                        PauseOnMute = true
                    };

                    _dbContext.ThemeConfigs.Add(newTheme);
                    await _dbContext.SaveChangesAsync();
                    MessageBox.Show("���ۻ���������������ӵ����ݿ⣡");
                }
                else
                {
                    MessageBox.Show($"��ȡ�� {themes.Count} ���������ݣ�");
                }
                // ���û��ƽ��䱳������
                DrawGradientBackground();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"���Գ���: {ex.Message}");
        }
    }
}
