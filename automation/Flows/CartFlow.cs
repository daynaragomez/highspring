using Highspring.Automation.Pages;

namespace Highspring.Automation.Flows;

public sealed class CartFlow(ProductsPage productsPage, CartPage cartPage)
{
    public void AddProductAndApplyCoupon(string baseUrl, string sku, string couponCode)
    {
        productsPage.Open(baseUrl);
        productsPage.WaitUntilLoaded();
        productsPage.AddToCart(sku);

        cartPage.Open(baseUrl);
        cartPage.WaitUntilLoaded();
        cartPage.ApplyCoupon(couponCode);
    }
}
