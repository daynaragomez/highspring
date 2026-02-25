using System;
using System.Text.Json;
using DemoShop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoShop.Web.Pages;

public class OrderConfirmationModel : PageModel
{
    private const string LastOrderSessionKey = "LAST_ORDER";

    [BindProperty(SupportsGet = true, Name = "orderId")]
    public string? OrderId { get; set; }

    public Order? Order { get; private set; }

    public IActionResult OnGet()
    {
        Order = LoadLastOrder();
        if (Order is null || !string.Equals(Order.OrderId, OrderId, StringComparison.OrdinalIgnoreCase))
        {
            Order = null;
            Response.StatusCode = 404;
        }

        return Page();
    }

    private Order? LoadLastOrder()
    {
        var json = HttpContext.Session.GetString(LastOrderSessionKey);
        if (string.IsNullOrEmpty(json))
        {
            return null;
        }

        return JsonSerializer.Deserialize<Order>(json);
    }
}
