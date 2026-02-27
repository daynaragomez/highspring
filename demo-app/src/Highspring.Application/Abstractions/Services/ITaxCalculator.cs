using Highspring.Application.UseCases;

namespace Highspring.Application.Abstractions.Services;

public interface ITaxCalculator
{
    Task<IReadOnlyList<TaxBreakdownLine>> CalculateAsync(decimal taxableAmount, string country, string stateOrRegion, CancellationToken cancellationToken);
}
