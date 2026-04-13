using Microsoft.EntityFrameworkCore;
using DagoShopFlow.Application.Domain;

namespace DagoShopFlow.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
}
