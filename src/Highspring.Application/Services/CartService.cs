using Highspring.Application.Abstractions.Repositories;
using Highspring.Application.Abstractions.Services;
using Highspring.Application.Domain;

namespace Highspring.Application.Services;

public class CartService(
    ICartRepository cartRepository,
    IProductRepository productRepository,
    IUnitOfWork unitOfWork) : ICartService
{
    public async Task<Cart> GetCartAsync(string guestSessionId, CancellationToken cancellationToken)
    {
        return await cartRepository.GetOrCreateAsync(guestSessionId, cancellationToken);
    }

    public async Task<Cart> AddItemAsync(string guestSessionId, string sku, int quantity, CancellationToken cancellationToken)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        }

        var product = await productRepository.GetBySkuAsync(sku, cancellationToken)
            ?? throw new InvalidOperationException($"Product with sku '{sku}' was not found.");

        var cart = await cartRepository.GetOrCreateAsync(guestSessionId, cancellationToken);
        var existing = cart.Items.FirstOrDefault(item => item.ProductId == product.Id);

        var nextQuantity = (existing?.Quantity ?? 0) + quantity;
        if (nextQuantity > product.StockQuantity)
        {
            throw new InvalidOperationException("Requested quantity exceeds available stock.");
        }

        if (existing is null)
        {
            cart.Items.Add(new CartItem
            {
                Id = Guid.NewGuid(),
                CartId = cart.Id,
                ProductId = product.Id,
                ProductSku = product.Sku,
                ProductName = product.Name,
                UnitPriceSnapshot = product.Price,
                Quantity = quantity
            });
        }
        else
        {
            existing.Quantity = nextQuantity;
        }

        cart.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await cartRepository.SaveAsync(cart, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return cart;
    }

    public async Task<Cart> SetItemQuantityAsync(string guestSessionId, string sku, int quantity, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(sku))
        {
            throw new ArgumentException("Sku is required.", nameof(sku));
        }

        if (quantity < 0)
        {
            throw new ArgumentException("Quantity cannot be negative.", nameof(quantity));
        }

        if (quantity == 0)
        {
            return await RemoveItemAsync(guestSessionId, sku, cancellationToken);
        }

        var product = await productRepository.GetBySkuAsync(sku, cancellationToken)
            ?? throw new InvalidOperationException($"Product with sku '{sku}' was not found.");

        if (quantity > product.StockQuantity)
        {
            throw new InvalidOperationException("Requested quantity exceeds available stock.");
        }

        var cart = await cartRepository.GetOrCreateAsync(guestSessionId, cancellationToken);
        var existing = cart.Items.FirstOrDefault(item => string.Equals(item.ProductSku, sku, StringComparison.OrdinalIgnoreCase));

        if (existing is null)
        {
            cart.Items.Add(new CartItem
            {
                Id = Guid.NewGuid(),
                CartId = cart.Id,
                ProductId = product.Id,
                ProductSku = product.Sku,
                ProductName = product.Name,
                UnitPriceSnapshot = product.Price,
                Quantity = quantity
            });
        }
        else
        {
            existing.Quantity = quantity;
            existing.ProductName = product.Name;
            existing.UnitPriceSnapshot = product.Price;
        }

        cart.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await cartRepository.SaveAsync(cart, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return cart;
    }

    public async Task<Cart> RemoveItemAsync(string guestSessionId, string sku, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetOrCreateAsync(guestSessionId, cancellationToken);
        cart.Items.RemoveAll(item => string.Equals(item.ProductSku, sku, StringComparison.OrdinalIgnoreCase));
        cart.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await cartRepository.SaveAsync(cart, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return cart;
    }
}
