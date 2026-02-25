using Highspring.Application.Abstractions.Repositories;
using Highspring.Application.Abstractions.Services;
using Highspring.Application.Common;
using Highspring.Application.Domain;

namespace Highspring.Application.Services;

public class DiscountService(ICouponRepository couponRepository) : IDiscountService
{
    public async Task<decimal> CalculateDiscountAsync(decimal subtotal, string? couponCode, CancellationToken cancellationToken)
    {
        if (subtotal <= 0 || string.IsNullOrWhiteSpace(couponCode))
        {
            return 0;
        }

        var coupon = await couponRepository.GetActiveByCodeAsync(couponCode.Trim(), DateTimeOffset.UtcNow, cancellationToken);
        if (coupon is null)
        {
            return 0;
        }

        var rawDiscount = coupon.Type switch
        {
            CouponType.Percent => subtotal * (coupon.Value / 100m),
            CouponType.Fixed => coupon.Value,
            _ => 0m
        };

        return Math.Min(MoneyMath.RoundCurrency(rawDiscount), subtotal);
    }
}
