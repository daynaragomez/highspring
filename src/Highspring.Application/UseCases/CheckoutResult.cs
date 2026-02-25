namespace Highspring.Application.UseCases;

public class CheckoutResult
{
    public Guid OrderId { get; set; }

    public required CartPricing Pricing { get; set; }
}
