using Highspring.Application.Abstractions.Repositories;
using Highspring.Application.Abstractions.Services;
using Highspring.Application.Common;
using Highspring.Application.UseCases;

namespace Highspring.Application.Services;

public class TaxCalculator(ITaxComponentRateRepository taxComponentRateRepository) : ITaxCalculator
{
    public async Task<IReadOnlyList<TaxBreakdownLine>> CalculateAsync(decimal taxableAmount, string country, string stateOrRegion, CancellationToken cancellationToken)
    {
        if (taxableAmount <= 0)
        {
            return [];
        }

        var rates = await taxComponentRateRepository.GetActiveRatesAsync(country, stateOrRegion, cancellationToken);

        return rates
            .OrderBy(rate => rate.Priority)
            .Select(rate => new TaxBreakdownLine
            {
                TaxCode = rate.TaxCode,
                Rate = rate.Rate,
                TaxableBase = taxableAmount,
                TaxAmount = MoneyMath.RoundCurrency(taxableAmount * rate.Rate)
            })
            .ToList();
    }
}
