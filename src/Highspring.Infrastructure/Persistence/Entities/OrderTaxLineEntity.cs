namespace Highspring.Infrastructure.Persistence.Entities;

public class OrderTaxLineEntity
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public required string TaxCode { get; set; }

    public decimal Rate { get; set; }

    public decimal TaxableBase { get; set; }

    public decimal TaxAmount { get; set; }

    public OrderEntity? Order { get; set; }
}
