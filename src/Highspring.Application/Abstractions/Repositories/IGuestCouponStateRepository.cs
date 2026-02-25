namespace Highspring.Application.Abstractions.Repositories;

public interface IGuestCouponStateRepository
{
    Task<string?> GetAppliedCouponCodeAsync(string guestSessionId, CancellationToken cancellationToken);

    Task SetAppliedCouponCodeAsync(string guestSessionId, string? couponCode, CancellationToken cancellationToken);
}
