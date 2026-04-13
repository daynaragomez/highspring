namespace DagoShopFlow.Application.Domain;

public class Cart
{
    public Guid Id { get; set; }

    public required string GuestSessionId { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }

    public DateTimeOffset UpdatedAtUtc { get; set; }

    public List<CartItem> Items { get; set; } = [];
}
