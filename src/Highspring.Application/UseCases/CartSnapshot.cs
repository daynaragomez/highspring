using Highspring.Application.Domain;

namespace Highspring.Application.UseCases;

public class CartSnapshot
{
    public required Cart Cart { get; set; }

    public required CartPricing Pricing { get; set; }

    public string? AppliedCouponCode { get; set; }
}
