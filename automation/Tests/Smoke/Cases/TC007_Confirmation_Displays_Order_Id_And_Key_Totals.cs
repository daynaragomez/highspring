using Highspring.Automation.Api;
using Highspring.Automation.Pages;
using Highspring.Automation.Tests;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace Highspring.Automation.Tests.Smoke.Cases;

public sealed class TC007_Confirmation_Displays_Order_Id_And_Key_Totals : BaseTestCase
{
    private readonly IWebDriver _driver;
    private readonly string _baseUrl;
    private readonly TestControlClient _api;
    private readonly ProductsPage _productsPage;
    private readonly CartPage _cartPage;
    private readonly CheckoutPage _checkoutPage;
    private readonly ConfirmationPage _confirmationPage;

    public TC007_Confirmation_Displays_Order_Id_And_Key_Totals(
        IWebDriver driver,
        WebDriverWait wait,
        string baseUrl,
        string apiBaseUrl,
        ILogger<TC007_Confirmation_Displays_Order_Id_And_Key_Totals> logger,
        Guid runId,
        Guid caseExecutionId)
        : base(logger, runId, caseExecutionId, "TC007")
    {
        _driver = driver;
        _baseUrl = baseUrl;
        _api = new TestControlClient(apiBaseUrl);
        _productsPage = new ProductsPage(driver, wait);
        _cartPage = new CartPage(driver, wait);
        _checkoutPage = new CheckoutPage(driver, wait);
        _confirmationPage = new ConfirmationPage(driver, wait);
    }

    public async Task PreconditionsAsync(CancellationToken cancellationToken = default)
    {
        LogInfo("Preconditions: confirming services are healthy, resetting baseline data, and preparing a successful checkout path.");
        await _api.ResetBaselineAsync(cancellationToken);
        LogInfo("Preconditions completed.");
    }

    public void Step1_OpenConfirmationState()
    {
        LogInfo("Step 1 — Open confirmation state: complete checkout flow to reach confirmation page. Expected: confirmation page loads.");

        _productsPage.Open(_baseUrl);
        _productsPage.WaitUntilLoaded();
        _productsPage.AddToCart("HOODIE-CLASSIC");

        _cartPage.WaitUntilLoaded();
        _cartPage.GoToCheckout();

        _checkoutPage.WaitUntilLoaded();
        _checkoutPage.FillAddressAndSubmit();
        _checkoutPage.WaitForCheckoutResult();

        _confirmationPage.WaitUntilLoaded();
        AssertUrlContains(_driver, "/checkout/confirmation/");

        LogInfo("Step 1 completed: confirmation page reached successfully.");
    }

    public void Step2_ValidateOrderId()
    {
        LogInfo("Step 2 — Validate order ID: read confirmation order identifier. Expected: non-empty ID is visible.");
        AssertHasValue(_confirmationPage.OrderId());
        LogInfo("Step 2 completed: non-empty order identifier is visible.");
    }

    public void Step3_ValidateKeyTotals()
    {
        LogInfo("Step 3 — Validate key totals: verify subtotal, discount, tax, and total fields are present and numeric.");

        var subtotal = _confirmationPage.Subtotal();
        var discount = _confirmationPage.Discount();
        var tax = _confirmationPage.Tax();
        var total = _confirmationPage.Total();

        Assert.True(subtotal >= 0m);
        Assert.True(discount >= 0m);
        Assert.True(tax >= 0m);
        Assert.True(total >= 0m);

        var expectedTotal = subtotal - discount + tax;
        Assert.Equal(expectedTotal, total);

        LogInfo("Step 3 completed: key totals are present, numeric, and coherent.");
    }

    public void ValidateExpectedResults()
    {
        LogInfo("Validating expected results: order identifier is visible and key totals are present/coherent.");

        AssertHasValue(_confirmationPage.OrderId());

        var subtotal = _confirmationPage.Subtotal();
        var discount = _confirmationPage.Discount();
        var tax = _confirmationPage.Tax();
        var total = _confirmationPage.Total();

        Assert.Equal(subtotal - discount + tax, total);

        LogInfo("Expected results validated successfully.");
    }
}
