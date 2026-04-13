using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using DagoShopFlow.Automation.Core;

namespace DagoShopFlow.Automation.Pages;

public abstract class BasePage(IWebDriver driver, WebDriverWait wait)
{
    protected IWebDriver Driver { get; } = driver;
    protected WebDriverWait Wait { get; } = wait;

    protected IWebElement FindByTestId(string testId)
        => Waits.ForTestId(Wait, testId);

    protected static decimal ParseCurrency(string value)
    {
        var normalized = value.Replace("$", string.Empty).Trim();
        return decimal.Parse(normalized, System.Globalization.CultureInfo.InvariantCulture);
    }
}
