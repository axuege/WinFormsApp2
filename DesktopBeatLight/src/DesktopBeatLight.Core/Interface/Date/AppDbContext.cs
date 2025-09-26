
// �����ռ䣺Core �������������Ŀ¼
namespace DesktopBeatLight.Core.Data;

using DesktopBeatLight.Core.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    // �������ñ�
    public DbSet<ThemeConfig> ThemeConfigs { get; set; } = null!;

    // ���� PostgreSQL ����
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;Database=WFApp;Username=postgres;Password=hyz123456;Port=5432"
            );
        }
    }

    // ʵ��ӳ������
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

        // ��ʼ��Ĭ������
        modelBuilder.Entity<ThemeConfig>().HasData(
            new ThemeConfig(1, "�����", true, "#FF4500", "#FFFF00", "#FFFFFF", "#333333", 8, LightPosition.Bottom, 80, 20, true),
            new ThemeConfig(2, "���", false, "#1E90FF", "#00BFFF", "#FFFFFF", "#222244", 8, LightPosition.Bottom, 70, 15, true),
            new ThemeConfig(3, "�޺���", false, "#9932CC", "#FF69B4", "#FFFFFF", "#330033", 8, LightPosition.Bottom, 85, 18, true)
        );
    }
}
}