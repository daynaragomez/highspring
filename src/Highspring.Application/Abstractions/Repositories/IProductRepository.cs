using Highspring.Application.Domain;

namespace Highspring.Application.Abstractions.Repositories;

public interface IProductRepository
{
    Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken);

    Task<IReadOnlyDictionary<Guid, Product>> GetByIdsAsync(IEnumerable<Guid> productIds, CancellationToken cancellationToken);

    Task UpdateStockAsync(Guid productId, int newStockQuantity, uint expectedVersion, CancellationToken cancellationToken);
}
