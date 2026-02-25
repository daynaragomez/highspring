using Highspring.Automation.Api;
using Highspring.Automation.Core;
using Highspring.Automation.Flows;
using Highspring.Automation.Pages;
using Xunit;

namespace Highspring.Automation.Tests.E2E;

public sealed class CheckoutE2ETests : BaseUiTest
{
    [Fact]
    [Trait("Category", "E2E")]
    public async Task E2E_Checkout_WithSave10_ValidatesTotals_And_ClearsCart()
    {
        var api = new TestControlClient(Settings.ApiBaseUrl);
        await api.ResetBaselineAsync();

        var products = new ProductsPage(Driver, Wait);
        var cart = new CartPage(Driver, Wait);
        var checkout = new CheckoutPage(Driver, Wait);
        var confirmation = new ConfirmationPage(Driver, Wait);
        var cartFlow = new CartFlow(products, cart);
        var checkoutFlow = new CheckoutFlow(checkout);

        cartFlow.AddProductAndApplyCoupon(Settings.BaseUrl, "HOODIE-CLASSIC", "SAVE10");

        Assert.Equal(49.00m, cart.Subtotal());
        Assert.Equal(4.90m, cart.Discount());
        Assert.Equal(6.61m, cart.Tax());
        Assert.Equal(50.71m, cart.Total());

        cart.GoToCheckout();
        checkout.WaitUntilLoaded();

        Assert.Equal(49.00m, checkout.Subtotal());
        Assert.Equal(4.90m, checkout.Discount());
        Assert.Equal(6.61m, checkout.Tax());
        Assert.Equal(50.71m, checkout.Total());

        checkoutFlow.SubmitOrder();
        checkout.WaitForCheckoutResult();

        confirmation.WaitUntilLoaded();
        Assert.False(string.IsNullOrWhiteSpace(confirmation.OrderId()));
        Assert.Equal(49.00m, confirmation.Subtotal());
        Assert.Equal(4.90m, confirmation.Discount());
        Assert.Equal(6.61m, confirmation.Tax());
        Assert.Equal(50.71m, confirmation.Total());

        cart.Open(Settings.BaseUrl);
        cart.WaitUntilLoaded();
        Assert.True(cart.ShowsEmptyCartMessage());
    }
}
