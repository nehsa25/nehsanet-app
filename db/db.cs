using Microsoft.EntityFrameworkCore;
using nehsanet_app.Models;
using LogLevel = nehsanet_app.Models.LogLevel;

namespace nehsanet_app.db
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<DBName> DBName { get; set; } = null!;
        public DbSet<DBAnimal> DBAnimal { get; set; } = null!;
        public DbSet<DBComment> DBComment { get; set; } = null!;
        public DbSet<DBPage> DBPage { get; set; } = null!;
        public DbSet<DBRelatedPage> DBRelatedPage { get; set; } = null!;
        public DbSet<Log> Logs { get; set; } = null!;
        public DbSet<LogLevel> LogLevels { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBName>()
                .HasOne(n => n.Animal)
                .WithMany()
                .HasForeignKey(n => n.SpiritAnimalID);

            modelBuilder.Entity<Log>()
                .HasOne(l => l.LogLevel)
                .WithMany()
                .HasForeignKey(l => l.Level);

            // many to many relationship between pages and related pages
            modelBuilder.Entity<DBRelatedPage>()
                .HasKey(pg => new { pg.page_id, pg.related_page_id });

            modelBuilder.Entity<DBRelatedPage>()
                .HasOne(pg => pg.Page)
                .WithMany(p => p.RelatedPages)
                .HasForeignKey(pg => pg.page_id);
        }

        public async Task<bool> CheckConnection()
        {
            await Task.Run(() =>
            {
                Database.OpenConnection();
                Database.CloseConnection();
            });
            return true;
        }
    }
}