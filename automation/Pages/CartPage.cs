using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Highspring.Automation.Selectors;

namespace Highspring.Automation.Pages;

public sealed class CartPage(IWebDriver driver, WebDriverWait wait) : BasePage(driver, wait)
{
    public void Open(string baseUrl) => Driver.Navigate().GoToUrl($"{baseUrl.TrimEnd('/')}/cart");

    public void WaitUntilLoaded() => FindByTestId(TestIds.PageCart);

    public void ApplyCoupon(string couponCode)
    {
        var input = FindByTestId(TestIds.ApplyDiscountInput);
        input.Clear();
        input.SendKeys(couponCode);
        FindByTestId(TestIds.ApplyDiscountButton).Click();
        WaitUntilLoaded();
    }

    public decimal Subtotal() => ParseCurrency(FindByTestId(TestIds.SubtotalValue).Text);
    public decimal Discount() => ParseCurrency(FindByTestId(TestIds.DiscountTotal).Text);
    public decimal Tax() => ParseCurrency(FindByTestId(TestIds.TaxTotal).Text);
    public decimal Total() => ParseCurrency(FindByTestId(TestIds.OrderTotal).Text);

    public void GoToCheckout()
    {
        Wait.Until(d => d.FindElement(By.CssSelector("a[href='/checkout']"))).Click();
    }

    public bool ShowsEmptyCartMessage()
    {
        return Driver.PageSource.Contains("Your cart is empty.", StringComparison.OrdinalIgnoreCase);
    }
}
