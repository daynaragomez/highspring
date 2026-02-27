using Highspring.Application.Abstractions.Services;
using Highspring.Application.Domain;
using Highspring.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Highspring.Web.Pages.Products;

public class IndexModel(IStorefrontService storefrontService) : PageModel
{
    public IReadOnlyList<Product> Products { get; private set; } = [];
    public string? ErrorMessage { get; private set; }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        Products = await storefrontService.GetProductsAsync(cancellationToken);
    }

    public async Task<IActionResult> OnPostAddToCartAsync(string sku, int quantity, CancellationToken cancellationToken)
    {
        var guestSessionId = GuestSessionAccessor.GetOrCreateGuestSessionId(HttpContext);

        try
        {
            await storefrontService.AddItemAsync(guestSessionId, sku, quantity <= 0 ? 1 : quantity, "CA", "QC", cancellationToken);
            return RedirectToPage("/Cart/Index");
        }
        catch (InvalidOperationException exception)
        {
            ErrorMessage = exception.Message;
            Products = await storefrontService.GetProductsAsync(cancellationToken);
            return Page();
        }
    }
}
