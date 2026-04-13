namespace DagoShopFlow.Application.Domain;

public class CartItem
{
    public Guid Id { get; set; }

    public Guid CartId { get; set; }

    public Guid ProductId { get; set; }

    public required string ProductSku { get; set; }

    public required string ProductName { get; set; }

    public decimal UnitPriceSnapshot { get; set; }

    public int Quantity { get; set; }
}
