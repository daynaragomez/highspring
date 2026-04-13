using Highspring.Application.Abstractions.Services;
using Highspring.Application.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagoShopFlow.Web.Pages.Checkout;

public class ConfirmationModel(IStorefrontService storefrontService) : PageModel
{
    public Order? Order { get; private set; }

    [BindProperty(SupportsGet = true)]
    public Guid OrderId { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        Order = await storefrontService.GetOrderAsync(OrderId, cancellationToken);
        return Order is null ? NotFound() : Page();
    }
}
