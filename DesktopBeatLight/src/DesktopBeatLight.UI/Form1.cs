using DesktopBeatLight.Core.Models;
using DesktopBeatLight.Core.Models.Enums;
using DesktopBeatLight.Audio.Abstractions;
using DesktopBeatLight.Core.IMplementations;
using DesktopBeatLight.Core.Abstractions;
using Microsoft.Extensions.Configuration;

namespace DesktopBeatLight.UI;

public partial class Form1 : Form
{
    // 注入的服务
    private readonly IAudioCapture _audioCapture;
    private readonly FftAnalyzer _fftAnalyzer;
    private readonly Func<string, IThemeRenderer> _themeRendererFactory;
    private readonly IConfiguration _configuration;

    // 私有字段
    private IThemeRenderer _currentThemeRenderer;
    private float[] _spectrumData = Array.Empty<float>();
    private bool _isRendering = false;
    private PeriodicTimer _renderTimer = null!; // 50FPS定时器
    private ThemeConfig _currentThemeConfig; // 当前主题配置

    // 构造函数
    public Form1(
        IAudioCapture audioCapture,
        FftAnalyzer fftAnalyzer,
        Func<string, IThemeRenderer> themeRendererFactory,
        IConfiguration configuration
    )
    {
        InitializeComponent();
        _audioCapture = audioCapture;
        _fftAnalyzer = fftAnalyzer;
        _themeRendererFactory = themeRendererFactory;
        _configuration = configuration;

        // 初始化主题配置
        _currentThemeConfig = CreateDefaultThemeConfig();
        _currentThemeRenderer = _themeRendererFactory("fire");
        _ = _currentThemeRenderer.InitializeAsync(); // 使用异步初始化，不等待完成

        // 设置窗口属性
        InitializeFormProperties();

        // 注册事件
        this.Load += Form1_Load;
        this.FormClosing += Form1_FormClosing;
        this.Paint += Form1_Paint;
    }

    // 初始化表单属性
    private void InitializeFormProperties()
    {
        // 窗口样式：无边框、置顶、透明背景
        this.FormBorderStyle = FormBorderStyle.None;
        this.TopMost = true;
        this.TransparencyKey = Color.Magenta; // 设置透明色
        this.BackColor = Color.Magenta;
        this.DoubleBuffered = true; // 启用双缓冲避免闪烁

        // 设置窗口位置和大小（桌面底部）
        var screen = Screen.PrimaryScreen;
        this.Width = screen.Bounds.Width;
        this.Height = _currentThemeConfig.LightHeight;
        this.Left = 0;
        this.Top = screen.Bounds.Height - this.Height;

        // 设置右键菜单
        InitializeContextMenu();
    }

    // 初始化右键菜单
    private void InitializeContextMenu()
    {
        ContextMenuStrip contextMenu = new ContextMenuStrip();

        // 主题菜单项
        ToolStripMenuItem themeMenu = new ToolStripMenuItem("主题");
        ToolStripMenuItem fireTheme = new ToolStripMenuItem("火焰主题");
        ToolStripMenuItem oceanTheme = new ToolStripMenuItem("海洋主题");
        ToolStripMenuItem neonTheme = new ToolStripMenuItem("霓虹主题");

        fireTheme.Click += (sender, e) => ChangeTheme("fire");
        oceanTheme.Click += (sender, e) => ChangeTheme("ocean");
        neonTheme.Click += (sender, e) => ChangeTheme("neon");

        themeMenu.DropDownItems.AddRange(new ToolStripMenuItem[] { fireTheme, oceanTheme, neonTheme });

        // 退出菜单项
        ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("退出");
        exitMenuItem.Click += (sender, e) => this.Close();

        // 添加菜单项到上下文菜单
        contextMenu.Items.AddRange(new ToolStripItem[] { themeMenu, exitMenuItem });

        // 设置表单的右键菜单
        this.ContextMenuStrip = contextMenu;
    }

    // 创建默认主题配置
    private ThemeConfig CreateDefaultThemeConfig()
    {
        return new ThemeConfig
        {
            ThemeConfigId = 1,
            Name = "火焰主题",
            IsDefault = true,
            PrimaryColor = "#FF4500",
            GradientEndColor = "#FFFF00",
            AccentColor = "#FFFFFF",
            MuteColor = "#333333",
            LightHeight = 8,
            LightPosition = LightPosition.Bottom,
            Brightness = 80,
            MuteBrightness = 20,
            PauseOnMute = true
        };
    }

    // 更改主题
    private void ChangeTheme(string themeName)
    {
        _currentThemeRenderer = _themeRendererFactory(themeName);
        _ = _currentThemeRenderer.InitializeAsync(); // 使用异步初始化，不等待完成
        this.Invalidate(); // 触发重绘
    }

    // 表单加载事件
    private async void Form1_Load(object sender, EventArgs e)
    {
        try
        {
            // 启动音频捕获
            await _audioCapture.StartCapture();
            _audioCapture.AudioDataAvailable += OnAudioDataAvailable;

            // 启动渲染循环
            await StartRenderLoop();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"初始化失败: {ex.Message}");
        }
    }

    // 音频数据可用事件处理程序
    private void OnAudioDataAvailable(ReadOnlySpan<float> audioData)
    {
        try
        {
            // 保存当前音频数据的副本，因为ReadOnlySpan可能在异步操作完成前被释放
            float[] audioDataCopy = audioData.ToArray();

            // 使用Task.Run在后台线程执行FFT分析，避免阻塞UI线程
            _ = Task.Run(async () =>
            {
                try
                {
                    // 调用异步版本的ProcessAudioData
                    float[] newSpectrumData = await _fftAnalyzer.ProcessAudioDataAsync(audioDataCopy);

                    // 在UI线程上更新spectrumData
                    this.Invoke((Action)(() =>
                    {
                        _spectrumData = newSpectrumData;
                    }));
                }
                catch (Exception ex)
                {
                    // 处理异步操作中的异常
                    Console.WriteLine($"异步处理音频数据时出错: {ex.Message}");
                }
            });
        }
        catch (Exception ex)
        {
            // 处理异常但不中断程序
            Console.WriteLine($"启动异步音频处理任务时出错: {ex.Message}");
        }
    }

    // 启动渲染循环
    private async Task StartRenderLoop()
    {
        if (_isRendering)
            return;

        _isRendering = true;
        _renderTimer = new PeriodicTimer(TimeSpan.FromMilliseconds(20)); // 50FPS

        try
        {
            while (await _renderTimer.WaitForNextTickAsync())
            {
                // 检查是否需要在静音时暂停渲染
                if (_audioCapture.IsMuted && _currentThemeConfig.PauseOnMute)
                {
                    if (_spectrumData.Length > 0)
                    {
                        // 淡出效果
                        for (int i = 0; i < _spectrumData.Length; i++)
                        {
                            _spectrumData[i] *= 0.95f;
                        }
                    }
                }

                // 触发重绘
                this.Invalidate();
            }
        }
        catch (ObjectDisposedException)
        {
            // 定时器已被释放，正常退出循环
        }
        finally
        {
            _isRendering = false;
        }
    }

    // 表单关闭事件处理程序
    private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        // 停止渲染循环
        _renderTimer?.Dispose();
        _isRendering = false;

        // 停止音频捕获
        _audioCapture.AudioDataAvailable -= OnAudioDataAvailable;
        await _audioCapture.StopCapture();
        _audioCapture.Dispose();
    }

    // 绘制事件处理程序
    private void Form1_Paint(object sender, PaintEventArgs e)
{
    if (_currentThemeRenderer == null)
        return;

    if (!this.IsHandleCreated || !this.Visible)
        return;

    var renderParams = new ThemeRenderParams
    {
        Graphics = e.Graphics,
        Bounds = this.ClientRectangle,
        SpectrumData = _spectrumData,
        ThemeConfig = _currentThemeConfig,
        IsMuted = _audioCapture.IsMuted,
        CurrentTime = DateTime.Now
    };

    try
    {
        // 直接在UI线程调用渲染方法
        _currentThemeRenderer.RenderAsync(renderParams).Wait();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"渲染过程中出错: {ex.Message}");
    }
}

    
}
