using System.ComponentModel.DataAnnotations;
using DagoShopFlow.Application.Abstractions.Services;
using DagoShopFlow.Application.UseCases;
using DagoShopFlow.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagoShopFlow.Web.Pages.Checkout;

public class IndexModel(IStorefrontService storefrontService) : PageModel
{
    public CartSnapshot Snapshot { get; private set; } = default!;

    [BindProperty]
    public CheckoutInput Input { get; set; } = new();

    public string? ErrorMessage { get; private set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        var guestSessionId = GuestSessionAccessor.GetOrCreateGuestSessionId(HttpContext);
        Snapshot = await storefrontService.GetCartSnapshotAsync(guestSessionId, "CA", "QC", cancellationToken);

        if (Snapshot.Cart.Items.Count == 0)
        {
            return RedirectToPage("/Cart/Index");
        }

        Input.Country = "CA";
        Input.StateOrRegion = "QC";

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        var guestSessionId = GuestSessionAccessor.GetOrCreateGuestSessionId(HttpContext);
        Snapshot = await storefrontService.GetCartSnapshotAsync(guestSessionId, Input.Country ?? "CA", Input.StateOrRegion ?? "QC", cancellationToken);

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var result = await storefrontService.CheckoutAsync(guestSessionId, new CheckoutAddress
            {
                FullName = Input.FullName ?? string.Empty,
                Email = Input.Email ?? string.Empty,
                Phone = Input.Phone ?? string.Empty,
                AddressLine1 = Input.AddressLine1 ?? string.Empty,
                AddressLine2 = Input.AddressLine2,
                City = Input.City ?? string.Empty,
                StateOrRegion = Input.StateOrRegion ?? "QC",
                PostalCode = Input.PostalCode ?? string.Empty,
                Country = Input.Country ?? "CA"
            }, null, cancellationToken);

            return RedirectToPage("/Checkout/Confirmation", new { orderId = result.OrderId });
        }
        catch (InvalidOperationException exception)
        {
            ErrorMessage = exception.Message;
            return Page();
        }
    }

    public class CheckoutInput
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string AddressLine1 { get; set; } = string.Empty;

        public string? AddressLine2 { get; set; }

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string StateOrRegion { get; set; } = "QC";

        [Required]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = "CA";
    }
}
