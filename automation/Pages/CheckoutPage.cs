using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Highspring.Automation.Selectors;

namespace Highspring.Automation.Pages;

public sealed class CheckoutPage(IWebDriver driver, WebDriverWait wait) : BasePage(driver, wait)
{
    public void WaitUntilLoaded() => FindByTestId(TestIds.PageCheckout);

    public decimal Subtotal() => ParseCurrency(FindByTestId(TestIds.SubtotalValue).Text);
    public decimal Discount() => ParseCurrency(FindByTestId(TestIds.DiscountTotal).Text);
    public decimal Tax() => ParseCurrency(FindByTestId(TestIds.TaxTotal).Text);
    public decimal Total() => ParseCurrency(FindByTestId(TestIds.OrderTotal).Text);

    public void FillAddressAndSubmit()
    {
        Fill("Input.FullName", "Automation User");
        Fill("Input.Email", "automation@example.com");
        Fill("Input.Phone", "5551234567");
        Fill("Input.AddressLine1", "123 Test St");
        Fill("Input.City", "Montreal");
        Fill("Input.StateOrRegion", "QC");
        Fill("Input.PostalCode", "H2H2H2");
        Fill("Input.Country", "CA");

        FindByTestId(TestIds.CheckoutSubmit).Click();
    }

    public void WaitForCheckoutResult()
    {
        Wait.Until(_ =>
            Driver.Url.Contains("/checkout/confirmation/", StringComparison.OrdinalIgnoreCase) ||
            Driver.PageSource.Contains("Order confirmed", StringComparison.OrdinalIgnoreCase));
    }

    private void Fill(string fieldName, string value)
    {
        var input = Driver.FindElement(By.Name(fieldName));
        input.Clear();
        input.SendKeys(value);
    }
}
