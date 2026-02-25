using Highspring.Application.Domain;

namespace Highspring.Application.Abstractions.Repositories;

public interface ICartRepository
{
    Task<Cart> GetOrCreateAsync(string guestSessionId, CancellationToken cancellationToken);

    Task<Cart?> GetByGuestSessionIdAsync(string guestSessionId, CancellationToken cancellationToken);

    Task SaveAsync(Cart cart, CancellationToken cancellationToken);
}
