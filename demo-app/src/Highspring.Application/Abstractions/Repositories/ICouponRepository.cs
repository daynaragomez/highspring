using Highspring.Application.Domain;

namespace Highspring.Application.Abstractions.Repositories;

public interface ICouponRepository
{
    Task<Coupon?> GetActiveByCodeAsync(string code, DateTimeOffset utcNow, CancellationToken cancellationToken);
}
