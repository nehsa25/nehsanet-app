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
        public DbSet<Page> DBPage { get; set; } = null!;
        public DbSet<RelatedPage> DBRelatedPage { get; set; } = null!;
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
                .HasForeignKey(l => l.Log_LogLevelID);

            modelBuilder.Entity<RelatedPage>()
                  .HasKey(rp => rp.Id);

            modelBuilder.Entity<RelatedPage>()
                .HasOne(rp => rp.Page)
                .WithMany(p => p.RelatedPagesNavigations)
                .HasForeignKey(rp => rp.PageId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RelatedPage>()
                .HasOne(rp => rp.RelatedPageNavigation)
                .WithMany(p => p.RelatedPages)
                .HasForeignKey(rp => rp.RelatedPageId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Page>()
                .HasIndex(p => p.Stem)
                .IsUnique();
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