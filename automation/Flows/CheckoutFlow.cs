using Highspring.Automation.Pages;

namespace Highspring.Automation.Flows;

public sealed class CheckoutFlow(CheckoutPage checkoutPage)
{
    public void SubmitOrder()
    {
        checkoutPage.FillAddressAndSubmit();
    }
}
