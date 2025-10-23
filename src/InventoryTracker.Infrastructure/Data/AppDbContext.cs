using Microsoft.EntityFrameworkCore;
using InventoryTracker.Domain.Entities;

namespace InventoryTracker.Infrastructure.Data
{
    /// <summary>
    /// پل ارتباطی EF Core با SQL Server.
    /// DbSet ها نمایانگر جدول‌ها هستند.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// options از DI (در Program.cs) تزریق می‌شود و شامل ConnectionString است.
        /// </summary>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// جدول محصولات. EF Core بر اساس این DbSet جدول Products را می‌سازد.
        /// </summary>
        public DbSet<Product> Products => Set<Product>();

        /// <summary>
        /// جای مناسب برای قاعده‌های مپینگ و Seed اولیه.
        /// در این نسخه‌ی پایه فقط ایندکس یکتا روی SKU می‌گذاریم.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique index روی SKU (برای جلوگیری از ثبت تکراری)
            modelBuilder.Entity<Product>()
                        .HasIndex(p => p.SKU)
                        .IsUnique();

            // تنظیم نوع ستون Price برای SQL Server
            // یعنی: مجموع 18 رقم، 2 رقم اعشار (مثلاً 999,999,999,999.99)
            modelBuilder.Entity<Product>()
                        .Property(p => p.Price)
                        .HasPrecision(18, 2);
        }

    }
}
