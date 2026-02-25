using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Highspring.Automation.Core;

public static class Waits
{
    public static IWebElement ForTestId(WebDriverWait wait, string testId)
        => wait.Until(d => d.FindElement(By.CssSelector($"[data-testid='{testId}']")));
}
