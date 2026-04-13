using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using DagoShopFlow.Automation.Config;

namespace DagoShopFlow.Automation.Core;

public abstract class BaseUiTest : IDisposable
{
    protected BaseUiTest()
    {
        Settings = TestSettings.Load();
        Driver = DriverFactory.Create(Settings);
        Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
    }

    protected TestSettings Settings { get; }
    protected ChromeDriver Driver { get; }
    protected WebDriverWait Wait { get; }

    public void Dispose()
    {
        Driver.Quit();
        Driver.Dispose();
    }
}
