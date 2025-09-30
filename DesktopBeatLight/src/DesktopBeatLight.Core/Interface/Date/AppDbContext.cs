
// 命名空间：Core 层的数据上下文目录
namespace DesktopBeatLight.Core.Data;

using DesktopBeatLight.Core.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
   //
   public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
   {
   }
   public DbSet<ThemeConfig> themeConfigs { get; set; }=null!;

}