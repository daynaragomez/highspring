namespace DagoShopFlow.Infrastructure.Persistence.Entities;

public class AppMetadata
{
    public Guid Id { get; set; }

    public required string Key { get; set; }

    public required string Value { get; set; }
}
