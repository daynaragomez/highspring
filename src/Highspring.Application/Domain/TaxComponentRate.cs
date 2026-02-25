namespace Highspring.Application.Domain;

public class TaxComponentRate
{
    public Guid Id { get; set; }

    public required string Country { get; set; }

    public required string StateOrRegion { get; set; }

    public required string TaxCode { get; set; }

    public decimal Rate { get; set; }

    public int Priority { get; set; }

    public bool IsActive { get; set; }
}
