namespace DagoShopFlow.Application.Domain;

public sealed class Cart
{
    public Guid Id { get; set; }
    public List<CartItem> Items { get; set; } = new();
}
