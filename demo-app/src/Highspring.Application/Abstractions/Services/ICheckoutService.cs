using Highspring.Application.UseCases;

namespace Highspring.Application.Abstractions.Services;

public interface ICheckoutService
{
    Task<CheckoutResult> CheckoutAsync(string guestSessionId, CheckoutAddress address, string? couponCode, CancellationToken cancellationToken);
}
