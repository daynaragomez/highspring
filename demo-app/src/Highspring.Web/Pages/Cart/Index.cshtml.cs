using Highspring.Application.Abstractions.Services;
using Highspring.Application.UseCases;
using Highspring.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Highspring.Web.Pages.Cart;

public class IndexModel(IStorefrontService storefrontService) : PageModel
{
    private const string DefaultCountry = "CA";
    private const string DefaultState = "QC";

    public CartSnapshot Snapshot { get; private set; } = default!;

    [BindProperty]
    public string? CouponCode { get; set; }

    public string? ErrorMessage { get; private set; }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        Snapshot = await GetSnapshotAsync(cancellationToken);
    }

    public async Task<IActionResult> OnPostUpdateQuantityAsync(string sku, int quantity, CancellationToken cancellationToken)
    {
        var guestSessionId = GuestSessionAccessor.GetOrCreateGuestSessionId(HttpContext);

        try
        {
            await storefrontService.SetItemQuantityAsync(guestSessionId, sku, quantity, DefaultCountry, DefaultState, cancellationToken);
            return RedirectToPage();
        }
        catch (InvalidOperationException exception)
        {
            Snapshot = await GetSnapshotAsync(cancellationToken);
            ErrorMessage = exception.Message;
            return Page();
        }
    }

    public async Task<IActionResult> OnPostRemoveItemAsync(string sku, CancellationToken cancellationToken)
    {
        var guestSessionId = GuestSessionAccessor.GetOrCreateGuestSessionId(HttpContext);
        await storefrontService.RemoveItemAsync(guestSessionId, sku, DefaultCountry, DefaultState, cancellationToken);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostApplyCouponAsync(CancellationToken cancellationToken)
    {
        var guestSessionId = GuestSessionAccessor.GetOrCreateGuestSessionId(HttpContext);

        try
        {
            await storefrontService.ApplyCouponAsync(guestSessionId, CouponCode, DefaultCountry, DefaultState, cancellationToken);
            return RedirectToPage();
        }
        catch (InvalidOperationException exception)
        {
            Snapshot = await GetSnapshotAsync(cancellationToken);
            ErrorMessage = exception.Message;
            return Page();
        }
    }

    private Task<CartSnapshot> GetSnapshotAsync(CancellationToken cancellationToken)
    {
        var guestSessionId = GuestSessionAccessor.GetOrCreateGuestSessionId(HttpContext);
        return storefrontService.GetCartSnapshotAsync(guestSessionId, DefaultCountry, DefaultState, cancellationToken);
    }
}
