using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace Highspring.Automation;

public sealed class SmokeTests
{
    [Fact]
    public void HomePage_Loads_And_ShowsHomeMarker()
    {
        var baseUrl = Environment.GetEnvironmentVariable("HIGHSPRING_BASE_URL") ?? "http://localhost:8080";
        var runHeadless = (Environment.GetEnvironmentVariable("HIGHSPRING_HEADLESS") ?? "true")
            .Equals("true", StringComparison.OrdinalIgnoreCase);

        var options = new ChromeOptions();
        if (runHeadless)
        {
            options.AddArgument("--headless=new");
        }

        options.AddArgument("--window-size=1400,1000");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--no-sandbox");

        using var driver = new ChromeDriver(options);
        driver.Navigate().GoToUrl(baseUrl);

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        var marker = wait.Until(d => d.FindElement(By.CssSelector("[data-testid='page-home']")));

        Assert.NotNull(marker);
        Assert.Contains("Highspring", driver.Title, StringComparison.OrdinalIgnoreCase);
    }
}
