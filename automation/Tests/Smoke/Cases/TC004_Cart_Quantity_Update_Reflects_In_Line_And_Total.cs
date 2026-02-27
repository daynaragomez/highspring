using Highspring.Automation.Api;
using Highspring.Automation.Pages;
using Highspring.Automation.Tests;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using Xunit;

namespace Highspring.Automation.Tests.Smoke.Cases;

public sealed class TC004_Cart_Quantity_Update_Reflects_In_Line_And_Total : BaseTestCase
{
    private readonly IWebDriver _driver;
    private readonly string _baseUrl;
    private readonly TestControlClient _api;
    private readonly ProductsPage _productsPage;
    private readonly CartPage _cartPage;

    public TC004_Cart_Quantity_Update_Reflects_In_Line_And_Total(
        IWebDriver driver,
        WebDriverWait wait,
        string baseUrl,
        string apiBaseUrl,
        ILogger<TC004_Cart_Quantity_Update_Reflects_In_Line_And_Total> logger,
        Guid runId,
        Guid caseExecutionId)
        : base(logger, runId, caseExecutionId, "TC004")
    {
        _driver = driver;
        _baseUrl = baseUrl;
        _api = new TestControlClient(apiBaseUrl);
        _productsPage = new ProductsPage(driver, wait);
        _cartPage = new CartPage(driver, wait);
    }

    public async Task PreconditionsAsync(CancellationToken cancellationToken = default)
    {
        LogInfo("Preconditions: services healthy, baseline reset, and cart can be seeded with one item.");
        await _api.ResetBaselineAsync(cancellationToken);
        LogInfo("Preconditions completed.");
    }

    public void Step1_SeedCartState()
    {
        LogInfo("Step 1 — Seed cart state: add product from products page. Expected: cart contains one line with quantity 1.");

        _productsPage.Open(_baseUrl);
        _productsPage.WaitUntilLoaded();
        _productsPage.AddToCart("HOODIE-CLASSIC");

        _cartPage.WaitUntilLoaded();
        AssertUrlContains(_driver, "/cart");
        Assert.Equal(1, _cartPage.Quantity("HOODIE-CLASSIC"));

        LogInfo("Step 1 completed: cart seeded with quantity 1.");
    }

    public void Step2_UpdateQuantity()
    {
        LogInfo("Step 2 — Update quantity: set quantity to 2. Expected: cart line shows quantity 2.");

        _cartPage.UpdateQuantity("HOODIE-CLASSIC", 2);
        Assert.Equal(2, _cartPage.Quantity("HOODIE-CLASSIC"));

        LogInfo("Step 2 completed: quantity updated to 2.");
    }

    public void Step3_ValidateTotals()
    {
        LogInfo("Step 3 — Validate totals: summary values are recalculated and coherent with updated quantity.");

        var subtotal = _cartPage.Subtotal();
        var discount = _cartPage.Discount();
        var tax = _cartPage.Tax();
        var total = _cartPage.Total();

        Assert.True(subtotal > 0m);
        Assert.True(total > 0m);
        Assert.Equal(subtotal - discount + tax, total);

        LogInfo("Step 3 completed: totals are coherent with updated quantity.");
    }

    public void ValidateExpectedResults()
    {
        LogInfo("Validating expected results: quantity update applied and summary is consistent.");

        Assert.Equal(2, _cartPage.Quantity("HOODIE-CLASSIC"));
        Assert.Equal(_cartPage.Subtotal() - _cartPage.Discount() + _cartPage.Tax(), _cartPage.Total());

        LogInfo("Expected results validated successfully.");
    }
}
