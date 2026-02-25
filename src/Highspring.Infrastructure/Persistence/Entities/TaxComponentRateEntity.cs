namespace Highspring.Infrastructure.Persistence.Entities;

public class TaxComponentRateEntity
{
    public Guid Id { get; set; }

    public required string Country { get; set; }

    public required string StateOrRegion { get; set; }

    public required string TaxCode { get; set; }

    public decimal Rate { get; set; }

    public int Priority { get; set; }

    public bool IsActive { get; set; }
}
