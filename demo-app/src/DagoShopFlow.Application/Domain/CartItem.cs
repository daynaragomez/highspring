namespace DagoShopFlow.Application.Domain;

public sealed class CartItem
{
    public string Sku { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
