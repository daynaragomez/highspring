using Highspring.Application.Abstractions.Repositories;
using Highspring.Application.Domain;
using Highspring.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Highspring.Infrastructure.Persistence.Repositories;

public class OrderRepository(AppDbContext dbContext) : IOrderRepository
{
    public Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        var entity = new OrderEntity
        {
            Id = order.Id,
            GuestSessionId = order.GuestSessionId,
            Subtotal = order.Subtotal,
            DiscountTotal = order.DiscountTotal,
            TaxTotal = order.TaxTotal,
            GrandTotal = order.GrandTotal,
            FullName = order.FullName,
            Email = order.Email,
            Phone = order.Phone,
            AddressLine1 = order.AddressLine1,
            AddressLine2 = order.AddressLine2,
            City = order.City,
            StateOrRegion = order.StateOrRegion,
            PostalCode = order.PostalCode,
            Country = order.Country,
            PlacedAtUtc = order.PlacedAtUtc,
            Items = order.Items.Select(item => new OrderItemEntity
            {
                Id = item.Id,
                OrderId = order.Id,
                ProductSku = item.ProductSku,
                ProductName = item.ProductName,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity,
                LineTotal = item.LineTotal
            }).ToList(),
            TaxLines = order.TaxLines.Select(item => new OrderTaxLineEntity
            {
                Id = item.Id,
                OrderId = order.Id,
                TaxCode = item.TaxCode,
                Rate = item.Rate,
                TaxableBase = item.TaxableBase,
                TaxAmount = item.TaxAmount
            }).ToList()
        };

        dbContext.Orders.Add(entity);
        return Task.CompletedTask;
    }

    public async Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Orders
            .AsNoTracking()
            .Include(item => item.Items)
            .Include(item => item.TaxLines)
            .SingleOrDefaultAsync(item => item.Id == orderId, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        return new Order
        {
            Id = entity.Id,
            GuestSessionId = entity.GuestSessionId,
            Subtotal = entity.Subtotal,
            DiscountTotal = entity.DiscountTotal,
            TaxTotal = entity.TaxTotal,
            GrandTotal = entity.GrandTotal,
            FullName = entity.FullName,
            Email = entity.Email,
            Phone = entity.Phone,
            AddressLine1 = entity.AddressLine1,
            AddressLine2 = entity.AddressLine2,
            City = entity.City,
            StateOrRegion = entity.StateOrRegion,
            PostalCode = entity.PostalCode,
            Country = entity.Country,
            PlacedAtUtc = entity.PlacedAtUtc,
            Items = entity.Items.Select(item => new OrderItem
            {
                Id = item.Id,
                OrderId = item.OrderId,
                ProductSku = item.ProductSku,
                ProductName = item.ProductName,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity,
                LineTotal = item.LineTotal
            }).ToList(),
            TaxLines = entity.TaxLines.Select(item => new OrderTaxLine
            {
                Id = item.Id,
                OrderId = item.OrderId,
                TaxCode = item.TaxCode,
                Rate = item.Rate,
                TaxableBase = item.TaxableBase,
                TaxAmount = item.TaxAmount
            }).ToList()
        };
    }
}
