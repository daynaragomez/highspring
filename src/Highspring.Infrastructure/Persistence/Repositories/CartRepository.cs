using Highspring.Application.Abstractions.Repositories;
using Highspring.Application.Domain;
using Highspring.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Highspring.Infrastructure.Persistence.Repositories;

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
        }

        return MapCart(cart);
    }

    public async Task<Cart?> GetByGuestSessionIdAsync(string guestSessionId, CancellationToken cancellationToken)
    {
        var cart = await GetEntityByGuestSessionIdAsync(guestSessionId, cancellationToken);
        return cart is null ? null : MapCart(cart);
    }

    public async Task SaveAsync(Cart cart, CancellationToken cancellationToken)
    {
        var entity = dbContext.Carts.Local.SingleOrDefault(item => item.Id == cart.Id);
        if (entity is null)
        {
            entity = await dbContext.Carts
                .Include(item => item.Items)
                .SingleOrDefaultAsync(item => item.Id == cart.Id, cancellationToken);
        }

        if (entity is null)
        {
            entity = new CartEntity
            {
                Id = cart.Id,
                GuestSessionId = cart.GuestSessionId,
                CreatedAtUtc = cart.CreatedAtUtc,
                UpdatedAtUtc = cart.UpdatedAtUtc
            };

            dbContext.Carts.Add(entity);
        }

        entity.GuestSessionId = cart.GuestSessionId;
        entity.CreatedAtUtc = cart.CreatedAtUtc;
        entity.UpdatedAtUtc = cart.UpdatedAtUtc;

        var existingByProductId = entity.Items.ToDictionary(item => item.ProductId);
        var incomingByProductId = cart.Items.ToDictionary(item => item.ProductId);

        var toRemove = entity.Items
            .Where(item => !incomingByProductId.ContainsKey(item.ProductId))
            .ToList();

        foreach (var item in toRemove)
        {
            entity.Items.Remove(item);
        }

        foreach (var incoming in cart.Items)
        {
            if (existingByProductId.TryGetValue(incoming.ProductId, out var existing))
            {
                existing.ProductSku = incoming.ProductSku;
                existing.ProductName = incoming.ProductName;
                existing.UnitPriceSnapshot = incoming.UnitPriceSnapshot;
                existing.Quantity = incoming.Quantity;
                continue;
            }

            entity.Items.Add(new CartItemEntity
            {
                Id = incoming.Id == Guid.Empty ? Guid.NewGuid() : incoming.Id,
                CartId = cart.Id,
                ProductId = incoming.ProductId,
                ProductSku = incoming.ProductSku,
                ProductName = incoming.ProductName,
                UnitPriceSnapshot = incoming.UnitPriceSnapshot,
                Quantity = incoming.Quantity
            });
        }

        await Task.CompletedTask;
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
