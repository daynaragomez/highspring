using Highspring.Automation.Api;
using Highspring.Automation.Pages;
using Highspring.Automation.Tests;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace Highspring.Automation.Tests.Smoke.Cases;

public sealed class TC005_Remove_Cart_Line_Shows_Empty_State : BaseTestCase
{
    private readonly IWebDriver _driver;
    private readonly string _baseUrl;
    private readonly TestControlClient _api;
    private readonly ProductsPage _productsPage;
    private readonly CartPage _cartPage;

    public TC005_Remove_Cart_Line_Shows_Empty_State(
        IWebDriver driver,
        WebDriverWait wait,
        string baseUrl,
        string apiBaseUrl,
        ILogger<TC005_Remove_Cart_Line_Shows_Empty_State> logger,
        Guid runId,
        Guid caseExecutionId)
        : base(logger, runId, caseExecutionId, "TC005")
    {
        _driver = driver;
        _baseUrl = baseUrl;
        _api = new TestControlClient(apiBaseUrl);
        _productsPage = new ProductsPage(driver, wait);
        _cartPage = new CartPage(driver, wait);
    }

    public async Task PreconditionsAsync(CancellationToken cancellationToken = default)
    {
        LogInfo("Preconditions: reset baseline and ensure cart can be seeded.");
        await _api.ResetBaselineAsync(cancellationToken);
        LogInfo("Preconditions completed.");
    }

    public void Step1_PrepareOneLineCart()
    {
        LogInfo("Step 1 — Prepare one-line cart. Expected: cart shows one line.");

        _productsPage.Open(_baseUrl);
        _productsPage.WaitUntilLoaded();
        _productsPage.AddToCart("HOODIE-CLASSIC");

        _cartPage.WaitUntilLoaded();
        AssertUrlContains(_driver, "/cart");
        Assert.Equal(1, _cartPage.Quantity("HOODIE-CLASSIC"));

        LogInfo("Step 1 completed: one-line cart prepared.");
    }

    public void Step2_RemoveLine()
    {
        LogInfo("Step 2 — Remove cart line. Expected: line is removed.");

        _cartPage.RemoveItem("HOODIE-CLASSIC");

        LogInfo("Step 2 completed: remove action executed.");
    }

    public void Step3_ValidateEmptyState()
    {
        LogInfo("Step 3 — Validate empty state. Expected: cart shows empty message.");

        Assert.True(_cartPage.ShowsEmptyCartMessage());

        LogInfo("Step 3 completed: empty cart message is visible.");
    }

    public void ValidateExpectedResults()
    {
        LogInfo("Validating expected results: line removed and empty state shown.");
        Assert.True(_cartPage.ShowsEmptyCartMessage());
        LogInfo("Expected results validated successfully.");
    }
}
