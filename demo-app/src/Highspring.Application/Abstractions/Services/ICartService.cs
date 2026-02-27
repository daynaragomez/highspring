using Highspring.Application.Domain;

namespace Highspring.Application.Abstractions.Services;

public interface ICartService
{
    Task<Cart> GetCartAsync(string guestSessionId, CancellationToken cancellationToken);

    Task<Cart> AddItemAsync(string guestSessionId, string sku, int quantity, CancellationToken cancellationToken);

    Task<Cart> SetItemQuantityAsync(string guestSessionId, string sku, int quantity, CancellationToken cancellationToken);

    Task<Cart> RemoveItemAsync(string guestSessionId, string sku, CancellationToken cancellationToken);
}
