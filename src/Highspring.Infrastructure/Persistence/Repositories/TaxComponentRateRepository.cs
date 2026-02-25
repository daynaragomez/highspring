using Highspring.Application.Abstractions.Repositories;
using Highspring.Application.Domain;
using Microsoft.EntityFrameworkCore;

namespace Highspring.Infrastructure.Persistence.Repositories;

public class TaxComponentRateRepository(AppDbContext dbContext) : ITaxComponentRateRepository
{
    public async Task<IReadOnlyList<TaxComponentRate>> GetActiveRatesAsync(string country, string stateOrRegion, CancellationToken cancellationToken)
    {
        var entities = await dbContext.TaxComponentRates
            .AsNoTracking()
            .Where(item => item.IsActive
                           && item.Country == country
                           && item.StateOrRegion == stateOrRegion)
            .OrderBy(item => item.Priority)
            .ToListAsync(cancellationToken);

        return entities.Select(item => new TaxComponentRate
        {
            Id = item.Id,
            Country = item.Country,
            StateOrRegion = item.StateOrRegion,
            TaxCode = item.TaxCode,
            Rate = item.Rate,
            Priority = item.Priority,
            IsActive = item.IsActive
        }).ToList();
    }
}
