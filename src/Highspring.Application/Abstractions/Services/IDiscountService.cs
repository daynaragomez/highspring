namespace Highspring.Application.Abstractions.Services;

public interface IDiscountService
{
    Task<decimal> CalculateDiscountAsync(decimal subtotal, string? couponCode, CancellationToken cancellationToken);
}
