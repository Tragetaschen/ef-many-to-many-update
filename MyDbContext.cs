using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Repro;

public class MyDbContext(SqliteConnection sqliteConnection) : DbContext
{
    public DbSet<One> Ones { get; set; } = null!;
    public DbSet<Two> Twos { get; set; } = null!;
    public DbSet<OneTwo> OneTwos { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlite(sqliteConnection);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<One>()
            .HasMany(o => o.Twos)
            .WithMany(t => t.Ones)
            .UsingEntity<OneTwo>(
                j =>
                {
                    j.Property(o => o.Order).HasDefaultValue(0);
                });
    }
}