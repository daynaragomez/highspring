using OpenQA.Selenium.Chrome;
using Highspring.Automation.Config;

namespace Highspring.Automation.Core;

public static class DriverFactory
{
    public static ChromeDriver Create(TestSettings settings)
    {
        var options = new ChromeOptions();

        if (settings.Headless)
        {
            options.AddArgument("--headless=new");
        }

        options.AddArgument("--window-size=1400,1000");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--no-sandbox");

        return new ChromeDriver(options);
    }
}
