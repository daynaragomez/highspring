namespace Highspring.Infrastructure.Persistence.Entities;

public class ProductEntity
{
    public Guid Id { get; set; }

    public required string Sku { get; set; }

    public required string Name { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public uint Version { get; set; }
}
