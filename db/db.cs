using app.models;
using Microsoft.EntityFrameworkCore;
using nehsanet_app.models;

namespace nehsanet_app.db
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<DBName> DBName { get; set; } = null!;
        public DbSet<DBAnimal> DBAnimal { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBName>()
                .HasOne(n => n.Animal)
                .WithMany()
                .HasForeignKey(n => n.SpiritAnimalID);
        }
    }
}