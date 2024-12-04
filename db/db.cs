using Microsoft.EntityFrameworkCore;
using nehsanet_app.models;

namespace nehsanet_app.db
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<DBName> DBName { get; set; } = null!;
    }
}