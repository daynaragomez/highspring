namespace Highspring.Application.UseCases;

public class TaxBreakdownLine
{
    public required string TaxCode { get; set; }

    public decimal Rate { get; set; }

    public decimal TaxableBase { get; set; }

    public decimal TaxAmount { get; set; }
}
