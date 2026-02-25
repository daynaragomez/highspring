using Highspring.Application.Domain;

namespace Highspring.Application.Abstractions.Services;

public interface ITestControlService
{
    Task ResetToBaselineAsync(CancellationToken cancellationToken);

    Task SetProductStockAsync(string sku, int quantity, CancellationToken cancellationToken);

    Task<Cart> SetCartItemsAsync(string guestSessionId, IReadOnlyList<TestCartItemInput> items, CancellationToken cancellationToken);

    Task SetCartCouponAsync(string guestSessionId, string? couponCode, CancellationToken cancellationToken);

    Task<Order?> GetOrderSnapshotAsync(Guid orderId, CancellationToken cancellationToken);
}

public sealed record TestCartItemInput(string Sku, int Quantity);
