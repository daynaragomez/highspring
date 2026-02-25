using Highspring.Application.Abstractions.Services;
using Highspring.Application.Domain;

namespace Highspring.Api;

public static class TestControlEndpoints
{
    public static IEndpointRouteBuilder MapTestControlEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/internal/test/v1").WithTags("TestControl");

        group.MapPost("/reset", async (ITestControlService service, CancellationToken cancellationToken) =>
        {
            await service.ResetToBaselineAsync(cancellationToken);
            return Results.NoContent();
        });

        group.MapPost("/products/{sku}/stock", async (string sku, SetProductStockRequest request, ITestControlService service, CancellationToken cancellationToken) =>
        {
            try
            {
                await service.SetProductStockAsync(sku, request.Quantity, cancellationToken);
                return Results.NoContent();
            }
            catch (ArgumentException exception)
            {
                return Results.BadRequest(new ErrorResponse(exception.Message));
            }
            catch (KeyNotFoundException exception)
            {
                return Results.NotFound(new ErrorResponse(exception.Message));
            }
        });

        group.MapPost("/carts/{guestSessionId}/items", async (string guestSessionId, SetCartItemsRequest request, ITestControlService service, CancellationToken cancellationToken) =>
        {
            try
            {
                var items = request.Items
                    .Select(item => new TestCartItemInput(item.Sku, item.Quantity))
                    .ToList();

                var cart = await service.SetCartItemsAsync(guestSessionId, items, cancellationToken);
                return Results.Ok(MapCart(cart));
            }
            catch (ArgumentException exception)
            {
                return Results.BadRequest(new ErrorResponse(exception.Message));
            }
            catch (KeyNotFoundException exception)
            {
                return Results.NotFound(new ErrorResponse(exception.Message));
            }
        });

        group.MapPost("/carts/{guestSessionId}/coupon", async (string guestSessionId, SetCartCouponRequest request, ITestControlService service, CancellationToken cancellationToken) =>
        {
            try
            {
                await service.SetCartCouponAsync(guestSessionId, request.CouponCode, cancellationToken);
                return Results.NoContent();
            }
            catch (ArgumentException exception)
            {
                return Results.BadRequest(new ErrorResponse(exception.Message));
            }
            catch (KeyNotFoundException exception)
            {
                return Results.NotFound(new ErrorResponse(exception.Message));
            }
        });

        group.MapGet("/orders/{orderId:guid}", async (Guid orderId, ITestControlService service, CancellationToken cancellationToken) =>
        {
            var order = await service.GetOrderSnapshotAsync(orderId, cancellationToken);
            return order is null ? Results.NotFound() : Results.Ok(MapOrder(order));
        });

        return endpoints;
    }

    private static CartResponse MapCart(Cart cart)
    {
        return new CartResponse(
            cart.Id,
            cart.GuestSessionId,
            cart.Items.Select(item => new CartItemResponse(item.ProductSku, item.ProductName, item.UnitPriceSnapshot, item.Quantity)).ToList());
    }

    private static OrderSnapshotResponse MapOrder(Order order)
    {
        return new OrderSnapshotResponse(
            order.Id,
            order.GuestSessionId,
            order.Subtotal,
            order.DiscountTotal,
            order.TaxTotal,
            order.GrandTotal,
            order.Items.Select(item => new OrderItemResponse(item.ProductSku, item.ProductName, item.UnitPrice, item.Quantity, item.LineTotal)).ToList(),
            order.TaxLines.Select(item => new OrderTaxLineResponse(item.TaxCode, item.Rate, item.TaxableBase, item.TaxAmount)).ToList(),
            order.PlacedAtUtc);
    }

    private sealed record SetProductStockRequest(int Quantity);

    private sealed record SetCartItemsRequest
    {
        public IReadOnlyList<SetCartItemRequest> Items { get; init; } = [];
    }

    private sealed record SetCartItemRequest(string Sku, int Quantity);

    private sealed record SetCartCouponRequest(string? CouponCode);

    private sealed record ErrorResponse(string Message);

    private sealed record CartResponse(Guid Id, string GuestSessionId, IReadOnlyList<CartItemResponse> Items);

    private sealed record CartItemResponse(string Sku, string Name, decimal UnitPrice, int Quantity);

    private sealed record OrderSnapshotResponse(
        Guid OrderId,
        string GuestSessionId,
        decimal Subtotal,
        decimal DiscountTotal,
        decimal TaxTotal,
        decimal GrandTotal,
        IReadOnlyList<OrderItemResponse> Items,
        IReadOnlyList<OrderTaxLineResponse> TaxLines,
        DateTimeOffset PlacedAtUtc);

    private sealed record OrderItemResponse(string ProductSku, string ProductName, decimal UnitPrice, int Quantity, decimal LineTotal);

    private sealed record OrderTaxLineResponse(string TaxCode, decimal Rate, decimal TaxableBase, decimal TaxAmount);
}
