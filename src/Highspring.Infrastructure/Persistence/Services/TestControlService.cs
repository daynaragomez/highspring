using System.Text.Json;
using Highspring.Application.Abstractions.Repositories;
using Highspring.Application.Abstractions.Services;
using Highspring.Application.Domain;
using Highspring.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Highspring.Infrastructure.Persistence.Services;

public class TestControlService(
    AppDbContext dbContext,
    IProductRepository productRepository,
    IOrderRepository orderRepository,
    IGuestCouponStateRepository guestCouponStateRepository,
    IUnitOfWork unitOfWork) : ITestControlService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task ResetToBaselineAsync(CancellationToken cancellationToken)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);

        await dbContext.OrderTaxLines.ExecuteDeleteAsync(cancellationToken);
        await dbContext.OrderItems.ExecuteDeleteAsync(cancellationToken);
        await dbContext.Orders.ExecuteDeleteAsync(cancellationToken);
        await dbContext.CartItems.ExecuteDeleteAsync(cancellationToken);
        await dbContext.Carts.ExecuteDeleteAsync(cancellationToken);
        await dbContext.Coupons.ExecuteDeleteAsync(cancellationToken);
        await dbContext.TaxComponentRates.ExecuteDeleteAsync(cancellationToken);
        await dbContext.Products.ExecuteDeleteAsync(cancellationToken);
        await dbContext.AppMetadata.ExecuteDeleteAsync(cancellationToken);

        var baselinePath = Path.Combine(AppContext.BaseDirectory, "Seed", "Baseline");

        var products = await ReadBaselineAsync<List<BaselineProduct>>(Path.Combine(baselinePath, "products.json"), cancellationToken);
        var coupons = await ReadBaselineAsync<List<BaselineCoupon>>(Path.Combine(baselinePath, "coupons.json"), cancellationToken);
        var taxRates = await ReadBaselineAsync<List<BaselineTaxRate>>(Path.Combine(baselinePath, "tax-component-rates.json"), cancellationToken);

        dbContext.Products.AddRange(products.Select(item => new ProductEntity
        {
            Id = Guid.NewGuid(),
            Sku = item.Sku,
            Name = item.Name,
            Price = item.Price,
            StockQuantity = item.StockQuantity
        }));

        dbContext.Coupons.AddRange(coupons.Select(item => new CouponEntity
        {
            Id = Guid.NewGuid(),
            Code = item.Code,
            Type = Enum.TryParse<CouponType>(item.Type, true, out var couponType) ? (int)couponType : (int)CouponType.Fixed,
            Value = item.Value,
            IsActive = item.IsActive,
            ValidFromUtc = item.ValidFromUtc,
            ValidToUtc = item.ValidToUtc
        }));

        dbContext.TaxComponentRates.AddRange(taxRates.Select(item => new TaxComponentRateEntity
        {
            Id = Guid.NewGuid(),
            Country = item.Country,
            StateOrRegion = item.StateOrRegion,
            TaxCode = item.TaxCode,
            Rate = item.Rate,
            Priority = item.Priority,
            IsActive = item.IsActive
        }));

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task SetProductStockAsync(string sku, int quantity, CancellationToken cancellationToken)
    {
        if (quantity < 0)
        {
            throw new ArgumentException("Stock quantity cannot be negative.", nameof(quantity));
        }

        var product = await dbContext.Products.SingleOrDefaultAsync(item => item.Sku == sku, cancellationToken)
            ?? throw new KeyNotFoundException($"Product '{sku}' was not found.");

        product.StockQuantity = quantity;
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<Cart> SetCartItemsAsync(string guestSessionId, IReadOnlyList<TestCartItemInput> items, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(guestSessionId))
        {
            throw new ArgumentException("Guest session id is required.", nameof(guestSessionId));
        }

        var cartEntity = await dbContext.Carts
            .Include(item => item.Items)
            .SingleOrDefaultAsync(item => item.GuestSessionId == guestSessionId, cancellationToken);

        if (cartEntity is null)
        {
            cartEntity = new CartEntity
            {
                Id = Guid.NewGuid(),
                GuestSessionId = guestSessionId,
                CreatedAtUtc = DateTimeOffset.UtcNow,
                UpdatedAtUtc = DateTimeOffset.UtcNow
            };

            dbContext.Carts.Add(cartEntity);
        }

        cartEntity.Items.Clear();

        var normalizedItems = items
            .GroupBy(item => item.Sku, StringComparer.OrdinalIgnoreCase)
            .Select(group => new TestCartItemInput(group.Key, group.Sum(item => item.Quantity)))
            .Where(item => item.Quantity > 0)
            .ToList();

        foreach (var item in normalizedItems)
        {
            var product = await productRepository.GetBySkuAsync(item.Sku, cancellationToken)
                ?? throw new KeyNotFoundException($"Product '{item.Sku}' was not found.");

            cartEntity.Items.Add(new CartItemEntity
            {
                Id = Guid.NewGuid(),
                CartId = cartEntity.Id,
                ProductId = product.Id,
                ProductSku = product.Sku,
                ProductName = product.Name,
                UnitPriceSnapshot = product.Price,
                Quantity = item.Quantity
            });
        }

        cartEntity.UpdatedAtUtc = DateTimeOffset.UtcNow;
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new Cart
        {
            Id = cartEntity.Id,
            GuestSessionId = cartEntity.GuestSessionId,
            CreatedAtUtc = cartEntity.CreatedAtUtc,
            UpdatedAtUtc = cartEntity.UpdatedAtUtc,
            Items = cartEntity.Items.Select(item => new CartItem
            {
                Id = item.Id,
                CartId = item.CartId,
                ProductId = item.ProductId,
                ProductSku = item.ProductSku,
                ProductName = item.ProductName,
                UnitPriceSnapshot = item.UnitPriceSnapshot,
                Quantity = item.Quantity
            }).ToList()
        };
    }

    public async Task SetCartCouponAsync(string guestSessionId, string? couponCode, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(guestSessionId))
        {
            throw new ArgumentException("Guest session id is required.", nameof(guestSessionId));
        }

        if (!string.IsNullOrWhiteSpace(couponCode))
        {
            var exists = await dbContext.Coupons
                .AsNoTracking()
                .AnyAsync(item => item.Code == couponCode.Trim() && item.IsActive, cancellationToken);

            if (!exists)
            {
                throw new KeyNotFoundException($"Coupon '{couponCode}' was not found or inactive.");
            }
        }

        await guestCouponStateRepository.SetAppliedCouponCodeAsync(guestSessionId, couponCode, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public Task<Order?> GetOrderSnapshotAsync(Guid orderId, CancellationToken cancellationToken)
    {
        return orderRepository.GetByIdAsync(orderId, cancellationToken);
    }

    private static async Task<T> ReadBaselineAsync<T>(string filePath, CancellationToken cancellationToken)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Baseline seed file not found: {filePath}");
        }

        await using var stream = File.OpenRead(filePath);
        var result = await JsonSerializer.DeserializeAsync<T>(stream, JsonOptions, cancellationToken);
        if (result is null)
        {
            throw new InvalidOperationException($"Unable to deserialize baseline seed file: {filePath}");
        }

        return result;
    }

    private sealed record BaselineProduct(string Sku, string Name, decimal Price, int StockQuantity);

    private sealed record BaselineCoupon(
        string Code,
        string Type,
        decimal Value,
        bool IsActive,
        DateTimeOffset? ValidFromUtc,
        DateTimeOffset? ValidToUtc);

    private sealed record BaselineTaxRate(string Country, string StateOrRegion, string TaxCode, decimal Rate, int Priority, bool IsActive);
}
