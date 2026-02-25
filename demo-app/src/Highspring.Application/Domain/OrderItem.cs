namespace Highspring.Application.Domain;

public class OrderItem
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public required string ProductSku { get; set; }

    public required string ProductName { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal LineTotal { get; set; }
}
