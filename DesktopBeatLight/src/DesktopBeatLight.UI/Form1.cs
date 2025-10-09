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
        // 在窗体加载时执行测试
        this.Load += Form1_Load;
    }
 

private void DrawGradientBackground()
{
    // 假设我们从数据库获取到了主题配置，这里先模拟使用之前定义的亮粉主题颜色
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
            MessageBox.Show($"数据库连接测试: {(canConnect ? "成功" : "失败")}");

            if (canConnect)
            {
                var themes =  _dbContext.ThemeConfigs.ToList();

                if (themes.Count == 0)
                {
                    MessageBox.Show("ThemeConfigs 表为空，添加一条亮粉活力主题数据。");

                    var newTheme = new ThemeConfig
                    {
                        ThemeConfigId=2,
                        Name = "亮粉活力主题",
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
                    MessageBox.Show("亮粉活力主题配置已添加到数据库！");
                }
                else
                {
                    MessageBox.Show($"读取到 {themes.Count} 条主题数据！");
                }
                // 调用绘制渐变背景方法
                DrawGradientBackground();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"测试出错: {ex.Message}");
        }
    }
}
