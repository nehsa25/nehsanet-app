using Microsoft.EntityFrameworkCore;
using nehsanet_app.Models;
using LogLevel = nehsanet_app.Models.LogLevel;

namespace nehsanet_app.db
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<DBName> DBName { get; set; } = null!;
        public DbSet<DBAnimal> DBAnimal { get; set; } = null!;
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
        }
    }
}