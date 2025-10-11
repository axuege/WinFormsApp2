using DesktopBeatLight.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DesktopBeatLight.Core.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public DbSet<ThemeConfig> ThemeConfigs { get; set; } = null!;

}