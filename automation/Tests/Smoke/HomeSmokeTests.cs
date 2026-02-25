using Highspring.Automation.Core;
using Highspring.Automation.Pages;
using Xunit;

namespace Highspring.Automation.Tests.Smoke;

public sealed class HomeSmokeTests : BaseUiTest
{
    [Fact]
    [Trait("Category", "Smoke")]
    public void Smoke_HomePage_Loads()
    {
        var home = new HomePage(Driver, Wait);

        home.Open(Settings.BaseUrl);
        home.WaitUntilLoaded();

        Assert.Contains("Highspring", home.Title, StringComparison.OrdinalIgnoreCase);
    }
}
