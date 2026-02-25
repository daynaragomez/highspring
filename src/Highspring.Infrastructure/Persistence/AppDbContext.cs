using Highspring.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Highspring.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AppMetadata> AppMetadata => Set<AppMetadata>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppMetadata>(entity =>
        {
            entity.ToTable("app_metadata");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Key)
                .HasMaxLength(100)
                .IsRequired();
            entity.Property(item => item.Value)
                .HasMaxLength(500)
                .IsRequired();
            entity.HasIndex(item => item.Key)
                .IsUnique();
        });

        base.OnModelCreating(modelBuilder);
    }
}
