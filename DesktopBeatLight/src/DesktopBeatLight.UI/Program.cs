using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DesktopBeatLight.Core.Data;
using DesktopBeatLight.Core.Abstractions;
using DesktopBeatLight.Core.IMplementations.ThemeRenderers;
using DesktopBeatLight.UI;
using System;
using System.Windows.Forms;
using DesktopBeatLight.Audio.Implementations;
using DesktopBeatLight.Audio.Abstractions;

var services = new ServiceCollection();

// 1. 加载配置
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// 2. 注册 DbContext
services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

// 3. 注册仓储
services.AddScoped<IThemeConfigRepository, ThemeConfigRepository>();
services.AddScoped<IAudioCapture, WasapiAudioCapture>();
// 4. 注册窗体
services.AddScoped<Form1>();

// 5. 构建容器并启动
using var serviceProvider = services.BuildServiceProvider();
// 确保数据库创建
ApplicationConfiguration.Initialize();
// 确保数据库创建
Application.Run(serviceProvider.GetRequiredService<Form1>());