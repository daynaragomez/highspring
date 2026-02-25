namespace Highspring.Infrastructure.Persistence.Entities;

public class CouponEntity
{
    public Guid Id { get; set; }

    public required string Code { get; set; }

    public int Type { get; set; }

    public decimal Value { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset? ValidFromUtc { get; set; }

    public DateTimeOffset? ValidToUtc { get; set; }
}
