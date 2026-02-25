using Highspring.Application.Domain;

namespace Highspring.Application.Abstractions.Repositories;

public interface ITaxComponentRateRepository
{
    Task<IReadOnlyList<TaxComponentRate>> GetActiveRatesAsync(string country, string stateOrRegion, CancellationToken cancellationToken);
}
