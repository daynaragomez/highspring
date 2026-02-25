using System.Linq;
using System.Text.Json;
using DemoShop.Web.Data;
using DemoShop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoShop.Web.Pages;

public class ProductDetailsModel : PageModel
{
    private const string CartSessionKey = "CART";

    private readonly AppDbContext _dbContext;

    public ProductDetailsModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public ProductViewModel? Product { get; private set; }

    [BindProperty]
    public int Quantity { get; set; } = 1;

    public IActionResult OnGet()
    {
        var product = FindProduct(Id);
        if (product is null)
        {
            return NotFound();
        }

        Product = product;
        if (Product.Stock <= 0)
        {
            ModelState.AddModelError(string.Empty, "This product is currently out of stock.");
        }
        if (Quantity < 1)
        {
            Quantity = 1;
        }

        return Page();
    }

    public IActionResult OnPost()
    {
        var product = FindProduct(Id);
        if (product is null)
        {
            return NotFound();
        }

        Product = product;

        if (Product.Stock <= 0)
        {
            ModelState.AddModelError(string.Empty, "This product is currently out of stock.");
        }
        if (Quantity < 1)
        {
            ModelState.AddModelError(nameof(Quantity), "Quantity must be at least 1.");
        }

        if (Quantity > Product.Stock)
        {
            ModelState.AddModelError(nameof(Quantity), $"Only {Product.Stock} left in stock.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var cart = GetCart();
        var existing = cart.FirstOrDefault(item => item.ProductId == product.Id);
        if (existing is not null)
        {
            existing.Quantity += Quantity;
        }
        else
        {
            cart.Add(new CartItem
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = Quantity
            });
        }

        SaveCart(cart);

        return Redirect("/cart");
    }

    private ProductViewModel? FindProduct(int id) => _dbContext.Products
        .Where(p => p.Id == id)
        .Select(p => new ProductViewModel(p.Id, p.Name, p.Price, p.Stock))
        .FirstOrDefault();

    private List<CartItem> GetCart()
    {
        var json = HttpContext.Session.GetString(CartSessionKey);
        if (string.IsNullOrEmpty(json))
        {
            return new List<CartItem>();
        }

        return JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
    }

    private void SaveCart(List<CartItem> cart)
    {
        var json = JsonSerializer.Serialize(cart);
        HttpContext.Session.SetString(CartSessionKey, json);
    }

    public record ProductViewModel(int Id, string Name, decimal Price, int Stock);
}
