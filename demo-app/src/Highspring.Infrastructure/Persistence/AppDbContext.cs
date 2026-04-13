using Highspring.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace DagoShopFlow.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ProductEntity> Products => Set<ProductEntity>();
    public DbSet<CartEntity> Carts => Set<CartEntity>();
    public DbSet<CartItemEntity> CartItems => Set<CartItemEntity>();
    public DbSet<CouponEntity> Coupons => Set<CouponEntity>();
    public DbSet<TaxComponentRateEntity> TaxComponentRates => Set<TaxComponentRateEntity>();
    public DbSet<OrderEntity> Orders => Set<OrderEntity>();
    public DbSet<OrderItemEntity> OrderItems => Set<OrderItemEntity>();
    public DbSet<OrderTaxLineEntity> OrderTaxLines => Set<OrderTaxLineEntity>();
    public DbSet<AppMetadata> AppMetadata => Set<AppMetadata>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.ToTable("products");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Sku).HasMaxLength(64).IsRequired();
            entity.Property(item => item.Name).HasMaxLength(200).IsRequired();
            entity.Property(item => item.Price).HasPrecision(18, 2);
            entity.Property(item => item.Version)
                .HasColumnName("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();
            entity.HasIndex(item => item.Sku).IsUnique();
        });

        modelBuilder.Entity<CartEntity>(entity =>
        {
            entity.ToTable("carts");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.GuestSessionId).HasMaxLength(120).IsRequired();
            entity.HasIndex(item => item.GuestSessionId).IsUnique();
            entity.HasMany(item => item.Items)
                .WithOne(item => item.Cart)
                .HasForeignKey(item => item.CartId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CartItemEntity>(entity =>
        {
            entity.ToTable("cart_items");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.ProductSku).HasMaxLength(64).IsRequired();
            entity.Property(item => item.ProductName).HasMaxLength(200).IsRequired();
            entity.Property(item => item.UnitPriceSnapshot).HasPrecision(18, 2);
            entity.HasIndex(item => new { item.CartId, item.ProductId }).IsUnique();
        });

        modelBuilder.Entity<CouponEntity>(entity =>
        {
            entity.ToTable("coupons");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Code).HasMaxLength(64).IsRequired();
            entity.Property(item => item.Value).HasPrecision(18, 2);
            entity.HasIndex(item => item.Code).IsUnique();
        });

        modelBuilder.Entity<TaxComponentRateEntity>(entity =>
        {
            entity.ToTable("tax_component_rates");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Country).HasMaxLength(2).IsRequired();
            entity.Property(item => item.StateOrRegion).HasMaxLength(16).IsRequired();
            entity.Property(item => item.TaxCode).HasMaxLength(16).IsRequired();
            entity.Property(item => item.Rate).HasPrecision(9, 6);
            entity.HasIndex(item => new { item.Country, item.StateOrRegion, item.TaxCode }).IsUnique();
        });

        modelBuilder.Entity<OrderEntity>(entity =>
        {
            entity.ToTable("orders");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.GuestSessionId).HasMaxLength(120).IsRequired();
            entity.Property(item => item.Subtotal).HasPrecision(18, 2);
            entity.Property(item => item.DiscountTotal).HasPrecision(18, 2);
            entity.Property(item => item.TaxTotal).HasPrecision(18, 2);
            entity.Property(item => item.GrandTotal).HasPrecision(18, 2);
            entity.Property(item => item.FullName).HasMaxLength(200).IsRequired();
            entity.Property(item => item.Email).HasMaxLength(320).IsRequired();
            entity.Property(item => item.Phone).HasMaxLength(30).IsRequired();
            entity.Property(item => item.AddressLine1).HasMaxLength(200).IsRequired();
            entity.Property(item => item.AddressLine2).HasMaxLength(200);
            entity.Property(item => item.City).HasMaxLength(120).IsRequired();
            entity.Property(item => item.StateOrRegion).HasMaxLength(16).IsRequired();
            entity.Property(item => item.PostalCode).HasMaxLength(20).IsRequired();
            entity.Property(item => item.Country).HasMaxLength(2).IsRequired();
            entity.HasMany(item => item.Items)
                .WithOne(item => item.Order)
                .HasForeignKey(item => item.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(item => item.TaxLines)
                .WithOne(item => item.Order)
                .HasForeignKey(item => item.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItemEntity>(entity =>
        {
            entity.ToTable("order_items");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.ProductSku).HasMaxLength(64).IsRequired();
            entity.Property(item => item.ProductName).HasMaxLength(200).IsRequired();
            entity.Property(item => item.UnitPrice).HasPrecision(18, 2);
            entity.Property(item => item.LineTotal).HasPrecision(18, 2);
        });

        modelBuilder.Entity<OrderTaxLineEntity>(entity =>
        {
            entity.ToTable("order_tax_lines");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.TaxCode).HasMaxLength(16).IsRequired();
            entity.Property(item => item.Rate).HasPrecision(9, 6);
            entity.Property(item => item.TaxableBase).HasPrecision(18, 2);
            entity.Property(item => item.TaxAmount).HasPrecision(18, 2);
        });

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
