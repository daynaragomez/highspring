using Highspring.Application.Abstractions.Repositories;
using Highspring.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Highspring.Infrastructure.Persistence.Repositories;

public class GuestCouponStateRepository(AppDbContext dbContext) : IGuestCouponStateRepository
{
    private const string KeyPrefix = "test:guest-coupon:";

    public async Task<string?> GetAppliedCouponCodeAsync(string guestSessionId, CancellationToken cancellationToken)
    {
        var key = BuildKey(guestSessionId);
        var metadata = await dbContext.AppMetadata
            .AsNoTracking()
            .SingleOrDefaultAsync(item => item.Key == key, cancellationToken);

        return metadata?.Value;
    }

    public async Task SetAppliedCouponCodeAsync(string guestSessionId, string? couponCode, CancellationToken cancellationToken)
    {
        var key = BuildKey(guestSessionId);
        var metadata = await dbContext.AppMetadata
            .SingleOrDefaultAsync(item => item.Key == key, cancellationToken);

        if (string.IsNullOrWhiteSpace(couponCode))
        {
            if (metadata is not null)
            {
                dbContext.AppMetadata.Remove(metadata);
            }

            return;
        }

        if (metadata is null)
        {
            dbContext.AppMetadata.Add(new AppMetadata
            {
                Id = Guid.NewGuid(),
                Key = key,
                Value = couponCode.Trim()
            });
            return;
        }

        metadata.Value = couponCode.Trim();
    }

    private static string BuildKey(string guestSessionId) => $"{KeyPrefix}{guestSessionId}";
}
