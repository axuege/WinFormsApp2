using DesktopBeatLight.Core.Abstractions;
using DesktopBeatLight.Core.IMplementations.ThemeRenderers;
using DesktopBeatLight.UI;
using DesktopBeatLight.Audio.Implementations;
using DesktopBeatLight.Audio.Abstractions;
using DesktopBeatLight.Core.IMplementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using DesktopBeatLight.Core.Data;

var services = new ServiceCollection();

// 1. 配置管理
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)  // 设置为必需
    .Build();

services.AddSingleton<IConfiguration>(configuration);

// 2. 配置数据库上下文
services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

// 3. 注册数据访问存储库
services.AddScoped<IThemeConfigRepository, ThemeConfigRepository>();

// 4. 注册主题渲染器（使用工厂模式）
services.AddSingleton<Func<string, IThemeRenderer>>(provider => themeName =>
{
    switch (themeName.ToLower())
    {
        case "ocean":
            return new OceanThemeRenderer();
        case "neon":
            return new NeonThemeRenderer();
        case "fire":
        default:
            return new FireThemeRenderer();
    }
});

// 5. 注册核心服务
services.AddSingleton<FftAnalyzer>();
services.AddSingleton<IAudioCapture, WasapiAudioCapture>();

// 6. 注册UI组件
services.AddScoped<Form1>();

// 5. 构建服务提供程序并运行应用程序
using var serviceProvider = services.BuildServiceProvider();
ApplicationConfiguration.Initialize();
Application.Run(serviceProvider.GetRequiredService<Form1>());