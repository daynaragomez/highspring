using DagoShopFlow.Application.Abstractions.Services;
using DagoShopFlow.Application.Domain;
using DagoShopFlow.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagoShopFlow.Web.Pages.Products;

public class DetailsModel(IStorefrontService storefrontService) : PageModel
{
    private const string DefaultCountry = "CA";
    private const string DefaultState = "QC";

    public Product? Product { get; private set; }

    public string? ErrorMessage { get; private set; }

    public int QuantityInCart { get; private set; }

    public bool IsInCart => QuantityInCart > 0;

    public int InputQuantity => IsInCart
        ? QuantityInCart
        : (Product is null || Product.StockQuantity <= 0 ? 0 : 1);

    public int RemainingStock => Product is null
        ? 0
        : Math.Max(0, Product.StockQuantity - QuantityInCart);

    [BindProperty(SupportsGet = true)]
    public string Sku { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        await LoadProductAndCartStateAsync(Sku, cancellationToken);
        return Product is null ? NotFound() : Page();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(string sku, int quantity, CancellationToken cancellationToken)
    {
        Sku = sku;
        await LoadProductAndCartStateAsync(Sku, cancellationToken);

        if (Product is null)
        {
            return NotFound();
        }

        var guestSessionId = GuestSessionAccessor.GetOrCreateGuestSessionId(HttpContext);
        var targetQuantity = Product.StockQuantity <= 0
            ? 0
            : Math.Clamp(quantity, 1, Product.StockQuantity);

        try
        {
            if (IsInCart)
            {
                await storefrontService.SetItemQuantityAsync(guestSessionId, sku, targetQuantity, DefaultCountry, DefaultState, cancellationToken);
            }
            else
            {
                await storefrontService.AddItemAsync(guestSessionId, sku, targetQuantity <= 0 ? 1 : targetQuantity, DefaultCountry, DefaultState, cancellationToken);
            }

            return RedirectToPage("/Cart/Index");
        }
        catch (InvalidOperationException exception)
        {
            await LoadProductAndCartStateAsync(Sku, cancellationToken);
            ErrorMessage = exception.Message;
            return Page();
        }
    }

    private async Task LoadProductAndCartStateAsync(string sku, CancellationToken cancellationToken)
    {
        Product = await storefrontService.GetProductBySkuAsync(sku, cancellationToken);
        QuantityInCart = 0;

        if (Product is null)
        {
            return;
        }

        var guestSessionId = GuestSessionAccessor.GetOrCreateGuestSessionId(HttpContext);
        var snapshot = await storefrontService.GetCartSnapshotAsync(guestSessionId, DefaultCountry, DefaultState, cancellationToken);
        QuantityInCart = snapshot.Cart.Items
            .Where(item => item.ProductSku == sku)
            .Select(item => item.Quantity)
            .FirstOrDefault();
    }
}
