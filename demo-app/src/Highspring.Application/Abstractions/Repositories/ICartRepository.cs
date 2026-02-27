using Highspring.Application.Domain;

namespace Highspring.Application.Abstractions.Repositories;

public interface ICartRepository
{
    Task<Cart> GetOrCreateAsync(string guestSessionId, CancellationToken cancellationToken);

    Task<Cart?> GetByGuestSessionIdAsync(string guestSessionId, CancellationToken cancellationToken);

    Task UpsertItemAsync(Guid cartId, Guid productId, string productSku, string productName, decimal unitPriceSnapshot, int quantity, CancellationToken cancellationToken);

    Task RemoveItemAsync(Guid cartId, string sku, CancellationToken cancellationToken);

    Task ClearItemsAsync(Guid cartId, CancellationToken cancellationToken);
}
