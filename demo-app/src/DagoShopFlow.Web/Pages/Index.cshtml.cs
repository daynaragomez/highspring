using DagoShopFlow.Application.Abstractions.Services;
using DagoShopFlow.Application.Domain;
using DagoShopFlow.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagoShopFlow.Web.Pages;

public class IndexModel(IStorefrontService storefrontService) : PageModel
{
    public IReadOnlyList<Product> Products { get; private set; } = [];

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        Products = await storefrontService.GetProductsAsync(cancellationToken);
    }

    public async Task<IActionResult> OnPostAddToCartAsync(string sku, int quantity, CancellationToken cancellationToken)
    {
        var guestSessionId = GuestSessionAccessor.GetOrCreateGuestSessionId(HttpContext);
        await storefrontService.AddItemAsync(guestSessionId, sku, quantity <= 0 ? 1 : quantity, "CA", "QC", cancellationToken);
        return RedirectToPage("/Cart/Index");
    }
}
