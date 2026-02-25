namespace Highspring.Application.Domain;

public class OrderTaxLine
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public required string TaxCode { get; set; }

    public decimal Rate { get; set; }

    public decimal TaxableBase { get; set; }

    public decimal TaxAmount { get; set; }
}
