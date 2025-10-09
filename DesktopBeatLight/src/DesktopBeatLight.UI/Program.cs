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

// 1. ��������
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// 2. ע�� DbContext
services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

// 3. ע��ִ�
services.AddScoped<IThemeConfigRepository, ThemeConfigRepository>();
services.AddScoped<IAudioCapture, WasapiAudioCapture>();
// 4. ע�ᴰ��
services.AddScoped<Form1>();

// 5. ��������������
using var serviceProvider = services.BuildServiceProvider();
// ȷ�����ݿⴴ��
ApplicationConfiguration.Initialize();
// ȷ�����ݿⴴ��
Application.Run(serviceProvider.GetRequiredService<Form1>());