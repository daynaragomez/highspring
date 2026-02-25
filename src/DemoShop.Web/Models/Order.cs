using System;

namespace DemoShop.Web.Models;

public class Order
{
    public string OrderId { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; }

    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;

    public string ShippingMethod { get; set; } = "Standard";
    public string PaymentMethod { get; set; } = "Card";

    public decimal Subtotal { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal Total { get; set; }

    public List<CartItem> Items { get; set; } = new();
}
