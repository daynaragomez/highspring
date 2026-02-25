using Highspring.Application.Abstractions.Repositories;
using Highspring.Application.Common;
using Highspring.Application.Domain;
using Microsoft.EntityFrameworkCore;

namespace Highspring.Infrastructure.Persistence.Repositories;

public class ProductRepository(AppDbContext dbContext) : IProductRepository
{
    public async Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(item => item.Sku == sku, cancellationToken);

        return entity is null ? null : MapProduct(entity);
    }

    public async Task<IReadOnlyDictionary<Guid, Product>> GetByIdsAsync(IEnumerable<Guid> productIds, CancellationToken cancellationToken)
    {
        var ids = productIds.Distinct().ToList();
        var entities = await dbContext.Products
            .AsNoTracking()
            .Where(item => ids.Contains(item.Id))
            .ToListAsync(cancellationToken);

        return entities.ToDictionary(item => item.Id, MapProduct);
    }

    public async Task UpdateStockAsync(Guid productId, int newStockQuantity, uint expectedVersion, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Products
            .SingleOrDefaultAsync(item => item.Id == productId, cancellationToken)
            ?? throw new InvalidOperationException("Product not found.");

        if (entity.Version != expectedVersion)
        {
            throw new ConcurrencyConflictException("Product stock changed by another transaction.");
        }

        entity.StockQuantity = newStockQuantity;
    }

    private static Product MapProduct(Persistence.Entities.ProductEntity entity)
    {
        return new Product
        {
            Id = entity.Id,
            Sku = entity.Sku,
            Name = entity.Name,
            Price = entity.Price,
            StockQuantity = entity.StockQuantity,
            Version = entity.Version
        };
    }
}
