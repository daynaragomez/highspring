using DagoShopFlow.Automation.Pages;

namespace DagoShopFlow.Automation.Flows;

public sealed class CheckoutFlow(CheckoutPage checkoutPage)
{
    public void SubmitOrder()
    {
        checkoutPage.FillAddressAndSubmit();
    }
}
