using Highspring.Application.Abstractions.Repositories;
using Highspring.Application.Domain;
using Highspring.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace DagoShopFlow.Infrastructure.Persistence.Repositories;

public class CartRepository(AppDbContext dbContext) : ICartRepository
{
    public async Task<Cart> GetOrCreateAsync(string guestSessionId, CancellationToken cancellationToken)
    {
        var cart = await GetEntityByGuestSessionIdAsync(guestSessionId, cancellationToken);
        if (cart is null)
        {
            cart = new CartEntity
            {
                Id = Guid.NewGuid(),
                GuestSessionId = guestSessionId,
                CreatedAtUtc = DateTimeOffset.UtcNow,
                UpdatedAtUtc = DateTimeOffset.UtcNow
            };

            dbContext.Carts.Add(cart);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return MapCart(cart);
    }

    public async Task<Cart?> GetByGuestSessionIdAsync(string guestSessionId, CancellationToken cancellationToken)
    {
        var cart = await GetEntityByGuestSessionIdAsync(guestSessionId, cancellationToken);
        return cart is null ? null : MapCart(cart);
    }

    public async Task UpsertItemAsync(Guid cartId, Guid productId, string productSku, string productName, decimal unitPriceSnapshot, int quantity, CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts.SingleAsync(item => item.Id == cartId, cancellationToken);

        var itemEntity = await dbContext.CartItems
            .SingleOrDefaultAsync(item => item.CartId == cartId && item.ProductId == productId, cancellationToken);

        if (itemEntity is null)
        {
            dbContext.CartItems.Add(new CartItemEntity
            {
                Id = Guid.NewGuid(),
                CartId = cartId,
                ProductId = productId,
                ProductSku = productSku,
                ProductName = productName,
                UnitPriceSnapshot = unitPriceSnapshot,
                Quantity = quantity
            });
        }
        else
        {
            itemEntity.ProductSku = productSku;
            itemEntity.ProductName = productName;
            itemEntity.UnitPriceSnapshot = unitPriceSnapshot;
            itemEntity.Quantity = quantity;
        }

        cart.UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    public async Task RemoveItemAsync(Guid cartId, string sku, CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(item => item.Items)
            .SingleAsync(item => item.Id == cartId, cancellationToken);

        var itemsToRemove = cart.Items
            .Where(item => string.Equals(item.ProductSku, sku, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (itemsToRemove.Count > 0)
        {
            dbContext.CartItems.RemoveRange(itemsToRemove);
        }

        cart.UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    public async Task ClearItemsAsync(Guid cartId, CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(item => item.Items)
            .SingleAsync(item => item.Id == cartId, cancellationToken);

        if (cart.Items.Count > 0)
        {
            dbContext.CartItems.RemoveRange(cart.Items);
        }

        cart.UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    private Task<CartEntity?> GetEntityByGuestSessionIdAsync(string guestSessionId, CancellationToken cancellationToken)
    {
        return dbContext.Carts
            .Include(item => item.Items)
            .SingleOrDefaultAsync(item => item.GuestSessionId == guestSessionId, cancellationToken);
    }

    private static Cart MapCart(CartEntity entity)
    {
        return new Cart
        {
            Id = entity.Id,
            GuestSessionId = entity.GuestSessionId,
            CreatedAtUtc = entity.CreatedAtUtc,
            UpdatedAtUtc = entity.UpdatedAtUtc,
            Items = entity.Items.Select(item => new CartItem
            {
                Id = item.Id,
                CartId = item.CartId,
                ProductId = item.ProductId,
                ProductSku = item.ProductSku,
                ProductName = item.ProductName,
                UnitPriceSnapshot = item.UnitPriceSnapshot,
                Quantity = item.Quantity
            }).ToList()
        };
    }
}
