
// 命名空间：Core 层的数据上下文目录
namespace DesktopBeatLight.Core.Data;

using DesktopBeatLight.Core.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    // 主题配置表
    public DbSet<ThemeConfig> ThemeConfigs { get; set; } = null!;

    // 配置 PostgreSQL 连接
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;Database=WFApp;Username=postgres;Password=hyz123456;Port=5432"
            );
        }
    }

    // 实体映射配置
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ThemeConfig>(entity =>
        {
            entity.HasKey(t => t.ThemeConfigId);
            entity.Property(t => t.ThemeConfigId).ValueGeneratedOnAdd();

            entity.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(t => t.PrimaryColor)
                .HasDefaultValue("#FF4500");
            entity.Property(t => t.GradientEndColor)
                .HasDefaultValue("#FFFF00");

            entity.Property(t => t.LightPosition)
                .HasConversion<string>();
        });

        // 初始化默认主题
        modelBuilder.Entity<ThemeConfig>().HasData(
            new ThemeConfig(1, "火焰橙", true, "#FF4500", "#FFFF00", "#FFFFFF", "#333333", 8, LightPosition.Bottom, 80, 20, true),
            new ThemeConfig(2, "深海蓝", false, "#1E90FF", "#00BFFF", "#FFFFFF", "#222244", 8, LightPosition.Bottom, 70, 15, true),
            new ThemeConfig(3, "霓虹紫", false, "#9932CC", "#FF69B4", "#FFFFFF", "#330033", 8, LightPosition.Bottom, 85, 18, true)
        );
    }
}
}