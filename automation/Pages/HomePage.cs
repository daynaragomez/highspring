using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Highspring.Automation.Selectors;

namespace Highspring.Automation.Pages;

public sealed class HomePage(IWebDriver driver, WebDriverWait wait) : BasePage(driver, wait)
{
    public void Open(string baseUrl) => Driver.Navigate().GoToUrl(baseUrl);

    public void WaitUntilLoaded() => FindByTestId(TestIds.PageHome);

    public string Title => Driver.Title;
}
