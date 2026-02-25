using System.Linq;
using System.Text.Json;
using DemoShop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoShop.Web.Pages;

public class CartModel : PageModel
{
    private const string CartSessionKey = "CART";

    public List<CartItem> CartItems { get; private set; } = new();

    public decimal Subtotal => CartItems.Sum(item => item.Price * item.Quantity);

    public void OnGet()
    {
        CartItems = LoadCart();
    }

    public IActionResult OnPostUpdate(int productId, int quantity)
    {
        CartItems = LoadCart();

        if (quantity < 1)
        {
            ModelState.AddModelError(string.Empty, "Quantity must be at least 1.");
            return Page();
        }

        var item = CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        if (item is null)
        {
            ModelState.AddModelError(string.Empty, "Item not found in cart.");
            return Page();
        }

        item.Quantity = quantity;
        PersistCart(CartItems);
        return RedirectToPage();
    }

    public IActionResult OnPostRemove(int productId)
    {
        CartItems = LoadCart();
        var item = CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        if (item is null)
        {
            return RedirectToPage();
        }

        CartItems.Remove(item);
        PersistCart(CartItems);
        return RedirectToPage();
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

    private void PersistCart(List<CartItem> cart)
    {
        if (cart.Count == 0)
        {
            HttpContext.Session.Remove(CartSessionKey);
            return;
        }

        var json = JsonSerializer.Serialize(cart);
        HttpContext.Session.SetString(CartSessionKey, json);
    }
}
