namespace DagoShopFlow.Infrastructure.Persistence.Entities;

public class CartEntity
{
    public Guid Id { get; set; }

    public required string GuestSessionId { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }

    public DateTimeOffset UpdatedAtUtc { get; set; }

    public List<CartItemEntity> Items { get; set; } = [];
}
