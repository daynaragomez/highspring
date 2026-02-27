using Highspring.Automation.Api;
using Highspring.Automation.Pages;
using Highspring.Automation.Tests;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace Highspring.Automation.Tests.E2E.Cases;

public sealed class TC104_Save10_Discount_Applies_Before_Tax_For_CA_QC : BaseTestCase
{
    private readonly IWebDriver _driver;
    private readonly string _baseUrl;
    private readonly TestControlClient _api;
    private readonly ProductsPage _productsPage;
    private readonly CartPage _cartPage;

    public TC104_Save10_Discount_Applies_Before_Tax_For_CA_QC(
        IWebDriver driver,
        WebDriverWait wait,
        string baseUrl,
        string apiBaseUrl,
        ILogger<TC104_Save10_Discount_Applies_Before_Tax_For_CA_QC> logger,
        Guid runId,
        Guid caseExecutionId)
        : base(logger, runId, caseExecutionId, "TC104")
    {
        _driver = driver;
        _baseUrl = baseUrl;
        _api = new TestControlClient(apiBaseUrl);
        _productsPage = new ProductsPage(driver, wait);
        _cartPage = new CartPage(driver, wait);
    }

    public async Task PreconditionsAsync(CancellationToken cancellationToken = default)
    {
        LogInfo("Preconditions: resetting baseline data and ensuring SAVE10 and target SKU are available.");
        await _api.ResetBaselineAsync(cancellationToken);
        LogInfo("Preconditions completed.");
    }

    public void Step1_AddItemAndApplyCoupon()
    {
        LogInfo("Step 1 — Add item and apply coupon: add HOODIE-CLASSIC and apply SAVE10. Expected: pricing recalculates.");
        _productsPage.Open(_baseUrl);
        _productsPage.WaitUntilLoaded();
        _productsPage.AddToCart("HOODIE-CLASSIC");

        _cartPage.WaitUntilLoaded();
        _cartPage.ApplyCoupon("SAVE10");

        AssertUrlContains(_driver, "/cart");
        LogInfo("Step 1 completed: item added and SAVE10 applied.");
    }

    public void Step2_ValidatePricingValues()
    {
        LogInfo("Step 2 — Validate pricing values: confirm subtotal, discount, tax, and total expected values.");

        Assert.Equal(49.00m, _cartPage.Subtotal());
        Assert.Equal(4.90m, _cartPage.Discount());
        Assert.Equal(6.61m, _cartPage.Tax());
        Assert.Equal(50.71m, _cartPage.Total());

        LogInfo("Step 2 completed: pricing values match expected deterministic baseline.");
    }

    public void Step3_ValidateTaxBasis()
    {
        LogInfo("Step 3 — Validate tax basis: verify tax is calculated after discount on discounted taxable base.");

        var subtotal = _cartPage.Subtotal();
        var discount = _cartPage.Discount();
        var tax = _cartPage.Tax();
        var total = _cartPage.Total();

        var taxableBase = subtotal - discount;
        Assert.Equal(44.10m, taxableBase);
        Assert.Equal(50.71m, taxableBase + tax);
        Assert.Equal(total, taxableBase + tax);

        LogInfo("Step 3 completed: tax basis and total relationship validated.");
    }

    public void ValidateExpectedResults()
    {
        LogInfo("Validating expected results: discount-before-tax rule and deterministic grand total behavior.");

        var subtotal = _cartPage.Subtotal();
        var discount = _cartPage.Discount();
        var tax = _cartPage.Tax();
        var total = _cartPage.Total();

        Assert.Equal(49.00m, subtotal);
        Assert.Equal(4.90m, discount);
        Assert.Equal(6.61m, tax);
        Assert.Equal(50.71m, total);
        Assert.Equal(44.10m, subtotal - discount);

        LogInfo("Expected results validated successfully.");
    }
}
