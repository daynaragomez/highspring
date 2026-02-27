using Highspring.Automation.Api;
using Highspring.Automation.Pages;
using Highspring.Automation.Tests;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace Highspring.Automation.Tests.E2E.Cases;

public sealed class TC112_Order_Snapshot_Matches_Confirmation_Totals : BaseTestCase
{
    private readonly IWebDriver _driver;
    private readonly string _baseUrl;
    private readonly TestControlClient _api;
    private readonly ProductsPage _productsPage;
    private readonly CartPage _cartPage;
    private readonly CheckoutPage _checkoutPage;
    private readonly ConfirmationPage _confirmationPage;
    private Guid _orderId;

    public TC112_Order_Snapshot_Matches_Confirmation_Totals(
        IWebDriver driver,
        WebDriverWait wait,
        string baseUrl,
        string apiBaseUrl,
        ILogger<TC112_Order_Snapshot_Matches_Confirmation_Totals> logger,
        Guid runId,
        Guid caseExecutionId)
        : base(logger, runId, caseExecutionId, "TC112")
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
        LogInfo("Preconditions: reset baseline and ensure checkout can produce an order ID.");
        await _api.ResetBaselineAsync(cancellationToken);
        LogInfo("Preconditions completed.");
    }

    public void Step1_CompleteCheckoutAndCaptureOrderId()
    {
        LogInfo("Step 1 — Complete checkout and capture order ID. Expected: non-empty order ID captured.");

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

        var orderIdText = _confirmationPage.OrderId();
        AssertHasValue(orderIdText);
        Assert.True(Guid.TryParse(orderIdText, out _orderId));

        LogInfo("Step 1 completed: order ID captured from confirmation page.");
    }

    public async Task Step2_QueryOrderSnapshotApiAsync(CancellationToken cancellationToken = default)
    {
        LogInfo("Step 2 — Query order snapshot API. Expected: snapshot response returned.");

        var snapshot = await _api.GetOrderSnapshotAsync(_orderId, cancellationToken);
        Assert.NotNull(snapshot);

        LogInfo("Step 2 completed: order snapshot response retrieved.");
    }

    public async Task Step3_CompareTotalsAsync(CancellationToken cancellationToken = default)
    {
        LogInfo("Step 3 — Compare totals between confirmation UI and persisted snapshot. Expected: subtotal/discount/tax/total match.");

        var snapshot = await _api.GetOrderSnapshotAsync(_orderId, cancellationToken);
        Assert.NotNull(snapshot);

        Assert.Equal(_confirmationPage.Subtotal(), snapshot!.Subtotal);
        Assert.Equal(_confirmationPage.Discount(), snapshot.DiscountTotal);
        Assert.Equal(_confirmationPage.Tax(), snapshot.TaxTotal);
        Assert.Equal(_confirmationPage.Total(), snapshot.GrandTotal);

        LogInfo("Step 3 completed: persisted snapshot totals match confirmation UI totals.");
    }

    public async Task ValidateExpectedResultsAsync(CancellationToken cancellationToken = default)
    {
        LogInfo("Validating expected results: persisted totals equal confirmation totals without mismatch.");

        var snapshot = await _api.GetOrderSnapshotAsync(_orderId, cancellationToken);
        Assert.NotNull(snapshot);

        Assert.Equal(_confirmationPage.Subtotal(), snapshot!.Subtotal);
        Assert.Equal(_confirmationPage.Discount(), snapshot.DiscountTotal);
        Assert.Equal(_confirmationPage.Tax(), snapshot.TaxTotal);
        Assert.Equal(_confirmationPage.Total(), snapshot.GrandTotal);

        LogInfo("Expected results validated successfully.");
    }
}
