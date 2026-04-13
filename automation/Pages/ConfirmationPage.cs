using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using DagoShopFlow.Automation.Selectors;

namespace DagoShopFlow.Automation.Pages;

public sealed class ConfirmationPage(IWebDriver driver, WebDriverWait wait) : BasePage(driver, wait)
{
    public void WaitUntilLoaded() => FindByTestId(TestIds.PageCheckoutConfirmation);

    public string OrderId() => FindByTestId(TestIds.OrderConfirmationId).Text.Trim();

    public decimal Subtotal() => ParseCurrency(FindByTestId(TestIds.SubtotalValue).Text);
    public decimal Discount() => ParseCurrency(FindByTestId(TestIds.DiscountTotal).Text);
    public decimal Tax() => ParseCurrency(FindByTestId(TestIds.TaxTotal).Text);
    public decimal Total() => ParseCurrency(FindByTestId(TestIds.OrderTotal).Text);
}
