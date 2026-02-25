using Highspring.Application.Abstractions.Repositories;
using Highspring.Application.Abstractions.Services;
using Highspring.Application.Common;
using Highspring.Application.Domain;
using Highspring.Application.UseCases;

namespace Highspring.Application.Services;

public class StorefrontService(
    IProductRepository productRepository,
    ICartService cartService,
    ICheckoutService checkoutService,
    IOrderRepository orderRepository,
    ICouponRepository couponRepository,
    IGuestCouponStateRepository guestCouponStateRepository,
    IDiscountService discountService,
    ITaxCalculator taxCalculator,
    IUnitOfWork unitOfWork) : IStorefrontService
{
    public Task<IReadOnlyList<Product>> GetProductsAsync(CancellationToken cancellationToken)
    {
        return productRepository.ListAsync(cancellationToken);
    }

    public Task<Product?> GetProductBySkuAsync(string sku, CancellationToken cancellationToken)
    {
        return productRepository.GetBySkuAsync(sku, cancellationToken);
    }

    public async Task<CartSnapshot> GetCartSnapshotAsync(string guestSessionId, string country, string stateOrRegion, CancellationToken cancellationToken)
    {
        var cart = await cartService.GetCartAsync(guestSessionId, cancellationToken);
        return await BuildSnapshotAsync(cart, guestSessionId, country, stateOrRegion, cancellationToken);
    }

    public async Task<CartSnapshot> AddItemAsync(string guestSessionId, string sku, int quantity, string country, string stateOrRegion, CancellationToken cancellationToken)
    {
        var cart = await cartService.AddItemAsync(guestSessionId, sku, quantity, cancellationToken);
        return await BuildSnapshotAsync(cart, guestSessionId, country, stateOrRegion, cancellationToken);
    }

    public async Task<CartSnapshot> SetItemQuantityAsync(string guestSessionId, string sku, int quantity, string country, string stateOrRegion, CancellationToken cancellationToken)
    {
        var cart = await cartService.SetItemQuantityAsync(guestSessionId, sku, quantity, cancellationToken);
        return await BuildSnapshotAsync(cart, guestSessionId, country, stateOrRegion, cancellationToken);
    }

    public async Task<CartSnapshot> RemoveItemAsync(string guestSessionId, string sku, string country, string stateOrRegion, CancellationToken cancellationToken)
    {
        var cart = await cartService.RemoveItemAsync(guestSessionId, sku, cancellationToken);
        return await BuildSnapshotAsync(cart, guestSessionId, country, stateOrRegion, cancellationToken);
    }

    public async Task<CartSnapshot> ApplyCouponAsync(string guestSessionId, string? couponCode, string country, string stateOrRegion, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(couponCode))
        {
            var activeCoupon = await couponRepository.GetActiveByCodeAsync(couponCode.Trim(), DateTimeOffset.UtcNow, cancellationToken);
            if (activeCoupon is null)
            {
                throw new InvalidOperationException($"Coupon '{couponCode}' is invalid or inactive.");
            }
        }

        await guestCouponStateRepository.SetAppliedCouponCodeAsync(guestSessionId, couponCode, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var cart = await cartService.GetCartAsync(guestSessionId, cancellationToken);
        return await BuildSnapshotAsync(cart, guestSessionId, country, stateOrRegion, cancellationToken);
    }

    public Task<CheckoutResult> CheckoutAsync(string guestSessionId, CheckoutAddress address, string? couponCode, CancellationToken cancellationToken)
    {
        return checkoutService.CheckoutAsync(guestSessionId, address, couponCode, cancellationToken);
    }

    public Task<Order?> GetOrderAsync(Guid orderId, CancellationToken cancellationToken)
    {
        return orderRepository.GetByIdAsync(orderId, cancellationToken);
    }

    private async Task<CartSnapshot> BuildSnapshotAsync(Cart cart, string guestSessionId, string country, string stateOrRegion, CancellationToken cancellationToken)
    {
        var appliedCouponCode = await guestCouponStateRepository.GetAppliedCouponCodeAsync(guestSessionId, cancellationToken);
        var subtotal = MoneyMath.RoundCurrency(cart.Items.Sum(item => item.UnitPriceSnapshot * item.Quantity));
        var discount = await discountService.CalculateDiscountAsync(subtotal, appliedCouponCode, cancellationToken);
        var taxable = MoneyMath.RoundCurrency(Math.Max(0, subtotal - discount));
        var taxLines = await taxCalculator.CalculateAsync(taxable, country, stateOrRegion, cancellationToken);
        var taxTotal = MoneyMath.RoundCurrency(taxLines.Sum(item => item.TaxAmount));
        var grandTotal = MoneyMath.RoundCurrency(taxable + taxTotal);

        return new CartSnapshot
        {
            Cart = cart,
            AppliedCouponCode = appliedCouponCode,
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
