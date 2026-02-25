using Highspring.Application.Abstractions.Services;
using Highspring.Application.Domain;
using Highspring.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Highspring.Web.Pages.Products;

public class DetailsModel(IStorefrontService storefrontService) : PageModel
{
    public Product? Product { get; private set; }

    public string? ErrorMessage { get; private set; }

    [BindProperty(SupportsGet = true)]
    public string Sku { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        Product = await storefrontService.GetProductBySkuAsync(Sku, cancellationToken);
        return Product is null ? NotFound() : Page();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(string sku, int quantity, CancellationToken cancellationToken)
    {
        Sku = sku;
        Product = await storefrontService.GetProductBySkuAsync(Sku, cancellationToken);

        if (Product is null)
        {
            return NotFound();
        }

        var guestSessionId = GuestSessionAccessor.GetOrCreateGuestSessionId(HttpContext);

        try
        {
            await storefrontService.AddItemAsync(guestSessionId, sku, quantity <= 0 ? 1 : quantity, "CA", "QC", cancellationToken);
            return RedirectToPage("/Cart/Index");
        }
        catch (InvalidOperationException exception)
        {
            ErrorMessage = exception.Message;
            return Page();
        }
    }
}
