using System;
using System.Linq;
using System.Text.Json;
using DemoShop.Web.Data;
using DemoShop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoShop.Web.Pages;

public class CheckoutModel : PageModel
{
    private const string CartSessionKey = "CART";
    private const string LastOrderSessionKey = "LAST_ORDER";
    private const decimal ExpressShippingCost = 9.99m;
    private readonly AppDbContext _dbContext;

    public CheckoutModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [BindProperty]
    public CheckoutInputModel Input { get; set; } = new();

    public List<CartItem> CartItems { get; private set; } = new();

    public decimal Subtotal => CartItems.Sum(item => item.Price * item.Quantity);

    public decimal ShippingCost => CartItems.Any() ? CalculateShippingCost(Input?.ShippingMethod) : 0m;

    public decimal Total => Subtotal + ShippingCost;

    public void OnGet()
    {
        CartItems = LoadCart();
        EnsureDefaults();
    }

    public IActionResult OnPost()
    {
        CartItems = LoadCart();
        EnsureDefaults();

        if (!CartItems.Any())
        {
            ModelState.Clear();
            return Page();
        }

        ValidateCartStock();

        if (!IsValidShipping(Input.ShippingMethod))
        {
            ModelState.AddModelError(nameof(Input.ShippingMethod), "Please choose a valid shipping method.");
        }

        if (!IsValidPayment(Input.PaymentMethod))
        {
            ModelState.AddModelError(nameof(Input.PaymentMethod), "Please choose a valid payment method.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var order = new Order
        {
            OrderId = Guid.NewGuid().ToString("N"),
            CreatedUtc = DateTime.UtcNow,
            FullName = Input.FullName,
            Email = Input.Email,
            Phone = Input.Phone,
            AddressLine1 = Input.AddressLine1,
            AddressLine2 = Input.AddressLine2,
            City = Input.City,
            PostalCode = Input.PostalCode,
            ShippingMethod = Input.ShippingMethod,
            PaymentMethod = Input.PaymentMethod,
            Subtotal = Subtotal,
            ShippingCost = ShippingCost,
            Total = Total,
            Items = CartItems.Select(item => new CartItem
            {
                ProductId = item.ProductId,
                Name = item.Name,
                Price = item.Price,
                Quantity = item.Quantity
            }).ToList()
        };

        SaveLastOrder(order);
        ClearCart();

        return Redirect($"/order/confirmation/{order.OrderId}");
    }

    private void EnsureDefaults()
    {
        Input ??= new CheckoutInputModel();
        if (string.IsNullOrWhiteSpace(Input.ShippingMethod) || !IsValidShipping(Input.ShippingMethod))
        {
            Input.ShippingMethod = "Standard";
        }

        if (string.IsNullOrWhiteSpace(Input.PaymentMethod) || !IsValidPayment(Input.PaymentMethod))
        {
            Input.PaymentMethod = "Card";
        }
    }

    private List<CartItem> LoadCart()
    {
        var json = HttpContext.Session.GetString(CartSessionKey);
        if (string.IsNullOrEmpty(json))
        {
            return new List<CartItem>();
        }

        return JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
    }

    private void ClearCart() => HttpContext.Session.Remove(CartSessionKey);

    private void SaveLastOrder(Order order)
    {
        var json = JsonSerializer.Serialize(order);
        HttpContext.Session.SetString(LastOrderSessionKey, json);
    }

    private void ValidateCartStock()
    {
        var productIds = CartItems.Select(ci => ci.ProductId).ToList();
        if (productIds.Count == 0)
        {
            return;
        }

        var inventory = _dbContext.Products
            .Where(p => productIds.Contains(p.Id))
            .ToDictionary(p => p.Id, p => p);

        foreach (var item in CartItems)
        {
            if (!inventory.TryGetValue(item.ProductId, out var product))
            {
                ModelState.AddModelError(string.Empty, $"{item.Name} is no longer available.");
                continue;
            }

            if (product.Stock <= 0)
            {
                ModelState.AddModelError(string.Empty, $"{product.Name} is out of stock.");
            }
            else if (item.Quantity > product.Stock)
            {
                ModelState.AddModelError(string.Empty, $"Only {product.Stock} units of {product.Name} are available.");
            }
        }
    }

    private static decimal CalculateShippingCost(string? method) =>
        string.Equals(method, "Express", StringComparison.OrdinalIgnoreCase) ? ExpressShippingCost : 0m;

    private static bool IsValidShipping(string? method) =>
        string.Equals(method, "Standard", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(method, "Express", StringComparison.OrdinalIgnoreCase);

    private static bool IsValidPayment(string? method) =>
        string.Equals(method, "Card", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(method, "CashOnDelivery", StringComparison.OrdinalIgnoreCase);
}
