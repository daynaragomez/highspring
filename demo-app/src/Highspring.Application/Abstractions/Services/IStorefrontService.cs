using Highspring.Application.Domain;
using Highspring.Application.UseCases;

namespace Highspring.Application.Abstractions.Services;

public interface IStorefrontService
{
    Task<IReadOnlyList<Product>> GetProductsAsync(CancellationToken cancellationToken);

    Task<Product?> GetProductBySkuAsync(string sku, CancellationToken cancellationToken);

    Task<CartSnapshot> GetCartSnapshotAsync(string guestSessionId, string country, string stateOrRegion, CancellationToken cancellationToken);

    Task<CartSnapshot> AddItemAsync(string guestSessionId, string sku, int quantity, string country, string stateOrRegion, CancellationToken cancellationToken);

    Task<CartSnapshot> SetItemQuantityAsync(string guestSessionId, string sku, int quantity, string country, string stateOrRegion, CancellationToken cancellationToken);

    Task<CartSnapshot> RemoveItemAsync(string guestSessionId, string sku, string country, string stateOrRegion, CancellationToken cancellationToken);

    Task<CartSnapshot> ApplyCouponAsync(string guestSessionId, string? couponCode, string country, string stateOrRegion, CancellationToken cancellationToken);

    Task<CheckoutResult> CheckoutAsync(string guestSessionId, CheckoutAddress address, string? couponCode, CancellationToken cancellationToken);

    Task<Order?> GetOrderAsync(Guid orderId, CancellationToken cancellationToken);
}
