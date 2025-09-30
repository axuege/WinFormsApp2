using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DesktopBeatLight.Core.Data;
using DesktopBeatLight.Core.Abstractions;
using DesktopBeatLight.Core.IMplementations.ThemeRenderers;
using DesktopBeatLight.UI;
using System;
using System.Windows.Forms;

var services = new ServiceCollection();

// 1. ¼ÓÔØÅäÖÃ
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// 2. ×¢²á DbContext
services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

// 3. ×¢²á²Ö´¢
services.AddScoped<IThemeConfigRepository, ThemeConfigRepository>();

// 4. ×¢²á´°Ìå
services.AddScoped<Form1>();

// 5. ¹¹½¨ÈÝÆ÷²¢Æô¶¯
using var serviceProvider = services.BuildServiceProvider();
ApplicationConfiguration.Initialize();
Application.Run(serviceProvider.GetRequiredService<Form1>());