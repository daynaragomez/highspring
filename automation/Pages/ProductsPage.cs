using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Highspring.Automation.Selectors;

namespace Highspring.Automation.Pages;

public sealed class ProductsPage(IWebDriver driver, WebDriverWait wait) : BasePage(driver, wait)
{
    public void Open(string baseUrl) => Driver.Navigate().GoToUrl($"{baseUrl.TrimEnd('/')}/products");

    public void WaitUntilLoaded() => FindByTestId(TestIds.PageProducts);

    public void AddToCart(string sku)
    {
        FindByTestId(TestIds.AddToCart(sku)).Click();
    }
}
