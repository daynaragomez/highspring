namespace Highspring.Infrastructure.Persistence.Entities;

public class OrderEntity
{
    public Guid Id { get; set; }

    public required string GuestSessionId { get; set; }

    public decimal Subtotal { get; set; }

    public decimal DiscountTotal { get; set; }

    public decimal TaxTotal { get; set; }

    public decimal GrandTotal { get; set; }

    public required string FullName { get; set; }

    public required string Email { get; set; }

    public required string Phone { get; set; }

    public required string AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public required string City { get; set; }

    public required string StateOrRegion { get; set; }

    public required string PostalCode { get; set; }

    public required string Country { get; set; }

    public DateTimeOffset PlacedAtUtc { get; set; }

    public List<OrderItemEntity> Items { get; set; } = [];

    public List<OrderTaxLineEntity> TaxLines { get; set; } = [];
}
