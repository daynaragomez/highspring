using Highspring.Application.Abstractions.Repositories;
using Highspring.Application.Domain;
using Microsoft.EntityFrameworkCore;

namespace DagoShopFlow.Infrastructure.Persistence.Repositories;

public class CouponRepository(AppDbContext dbContext) : ICouponRepository
{
    public async Task<Coupon?> GetActiveByCodeAsync(string code, DateTimeOffset utcNow, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Coupons
            .AsNoTracking()
            .SingleOrDefaultAsync(item => item.Code == code
                                          && item.IsActive
                                          && (item.ValidFromUtc == null || item.ValidFromUtc <= utcNow)
                                          && (item.ValidToUtc == null || item.ValidToUtc >= utcNow),
                cancellationToken);

        if (entity is null)
        {
            return null;
        }

        return new Coupon
        {
            Id = entity.Id,
            Code = entity.Code,
            Type = (CouponType)entity.Type,
            Value = entity.Value,
            IsActive = entity.IsActive,
            ValidFromUtc = entity.ValidFromUtc,
            ValidToUtc = entity.ValidToUtc
        };
    }
}
