using Highspring.Application.Abstractions.Repositories;
using Highspring.Application.Abstractions.Services;
using Highspring.Application.Common;
using Highspring.Application.Domain;
using Highspring.Application.UseCases;

namespace Highspring.Application.Services;

public class CheckoutService(
    ICartRepository cartRepository,
    IProductRepository productRepository,
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork,
    IDiscountService discountService,
    ITaxCalculator taxCalculator) : ICheckoutService
{
    public async Task<CheckoutResult> CheckoutAsync(string guestSessionId, CheckoutAddress address, string? couponCode, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetByGuestSessionIdAsync(guestSessionId, cancellationToken)
            ?? throw new InvalidOperationException("Cart not found.");

        if (cart.Items.Count == 0)
        {
            throw new InvalidOperationException("Cart is empty.");
        }

        var productIds = cart.Items.Select(item => item.ProductId).Distinct().ToList();
        var products = await productRepository.GetByIdsAsync(productIds, cancellationToken);

        foreach (var item in cart.Items)
        {
            if (!products.TryGetValue(item.ProductId, out var product))
            {
                throw new InvalidOperationException($"Product '{item.ProductSku}' is unavailable.");
            }

            if (product.StockQuantity < item.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock for '{item.ProductSku}'.");
            }
        }

        var subtotal = MoneyMath.RoundCurrency(cart.Items.Sum(item => item.UnitPriceSnapshot * item.Quantity));
        var discount = await discountService.CalculateDiscountAsync(subtotal, couponCode, cancellationToken);
        var taxableAmount = MoneyMath.RoundCurrency(Math.Max(0, subtotal - discount));
        var taxLines = await taxCalculator.CalculateAsync(taxableAmount, address.Country, address.StateOrRegion, cancellationToken);
        var taxTotal = MoneyMath.RoundCurrency(taxLines.Sum(line => line.TaxAmount));
        var grandTotal = MoneyMath.RoundCurrency(taxableAmount + taxTotal);

        foreach (var item in cart.Items)
        {
            var product = products[item.ProductId];
            var newStock = product.StockQuantity - item.Quantity;
            await productRepository.UpdateStockAsync(product.Id, newStock, product.Version, cancellationToken);
        }

        var order = new Order
        {
            Id = Guid.NewGuid(),
            GuestSessionId = guestSessionId,
            Subtotal = subtotal,
            DiscountTotal = discount,
            TaxTotal = taxTotal,
            GrandTotal = grandTotal,
            FullName = address.FullName,
            Email = address.Email,
            Phone = address.Phone,
            AddressLine1 = address.AddressLine1,
            AddressLine2 = address.AddressLine2,
            City = address.City,
            StateOrRegion = address.StateOrRegion,
            PostalCode = address.PostalCode,
            Country = address.Country,
            PlacedAtUtc = DateTimeOffset.UtcNow,
            Items = cart.Items.Select(item => new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = Guid.Empty,
                ProductSku = item.ProductSku,
                ProductName = item.ProductName,
                UnitPrice = item.UnitPriceSnapshot,
                Quantity = item.Quantity,
                LineTotal = MoneyMath.RoundCurrency(item.UnitPriceSnapshot * item.Quantity)
            }).ToList(),
            TaxLines = taxLines.Select(line => new OrderTaxLine
            {
                Id = Guid.NewGuid(),
                OrderId = Guid.Empty,
                TaxCode = line.TaxCode,
                Rate = line.Rate,
                TaxableBase = line.TaxableBase,
                TaxAmount = line.TaxAmount
            }).ToList()
        };

        await orderRepository.AddAsync(order, cancellationToken);

        cart.Items.Clear();
        cart.UpdatedAtUtc = DateTimeOffset.UtcNow;
        await cartRepository.SaveAsync(cart, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CheckoutResult
        {
            OrderId = order.Id,
            Pricing = new CartPricing
            {
                Subtotal = subtotal,
                DiscountTotal = discount,
                TaxTotal = taxTotal,
                GrandTotal = grandTotal,
                TaxLines = taxLines.ToList()
            }
        };
    }
}
