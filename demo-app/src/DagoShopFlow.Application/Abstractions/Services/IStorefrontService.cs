using DagoShopFlow.Application.Domain;
using System.Threading;

namespace DagoShopFlow.Application.Abstractions.Services;

public interface IStorefrontService
{
    Task<IReadOnlyList<Product>> GetProductsAsync(CancellationToken cancellationToken = default);
    Task AddItemAsync(Guid guestSessionId, string sku, int quantity, string country, string stateOrRegion, CancellationToken cancellationToken = default);
    Task<CartSnapshot> GetCartSnapshotAsync(Guid guestSessionId, string country, string stateOrRegion, CancellationToken cancellationToken = default);
    Task<CheckoutResult> CheckoutAsync(Guid guestSessionId, CheckoutAddress address, object? options, CancellationToken cancellationToken = default);
}
