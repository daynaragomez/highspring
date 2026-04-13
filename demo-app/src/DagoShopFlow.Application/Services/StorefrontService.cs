using DagoShopFlow.Application.Abstractions.Services;
using DagoShopFlow.Application.Domain;
using DagoShopFlow.Application.UseCases;

namespace DagoShopFlow.Application.Services;

public sealed class StorefrontService : IStorefrontService
{
    public Task AddItemAsync(Guid guestSessionId, string sku, int quantity, string country, string stateOrRegion, CancellationToken cancellationToken = default)
    {
        // no-op stub for standalone project
        return Task.CompletedTask;
    }

    public Task<CartSnapshot> GetCartSnapshotAsync(Guid guestSessionId, string country, string stateOrRegion, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new CartSnapshot());
    }

    public Task<IReadOnlyList<Product>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = new List<Product>
        {
            new Product { Id = Guid.NewGuid(), Sku = "HOODIE-CLASSIC", Name = "Classic Hoodie", Price = 49.99m }
        };
        return Task.FromResult<IReadOnlyList<Product>>(products);
    }

    public Task<CheckoutResult> CheckoutAsync(Guid guestSessionId, CheckoutAddress address, object? options, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new CheckoutResult { OrderId = Guid.NewGuid() });
    }
}
