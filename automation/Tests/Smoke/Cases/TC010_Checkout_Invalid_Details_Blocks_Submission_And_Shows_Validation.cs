using Highspring.Automation.Api;
using Highspring.Automation.Pages;
using Highspring.Automation.Tests;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace Highspring.Automation.Tests.Smoke.Cases;

public sealed class TC010_Checkout_Invalid_Details_Blocks_Submission_And_Shows_Validation : BaseTestCase
{
    private readonly IWebDriver _driver;
    private readonly string _baseUrl;
    private readonly TestControlClient _api;
    private readonly ProductsPage _productsPage;
    private readonly CartPage _cartPage;
    private readonly CheckoutPage _checkoutPage;

    public TC010_Checkout_Invalid_Details_Blocks_Submission_And_Shows_Validation(
        IWebDriver driver,
        WebDriverWait wait,
        string baseUrl,
        string apiBaseUrl,
        ILogger<TC010_Checkout_Invalid_Details_Blocks_Submission_And_Shows_Validation> logger,
        Guid runId,
        Guid caseExecutionId)
        : base(logger, runId, caseExecutionId, "TC010")
    {
        _driver = driver;
        _baseUrl = baseUrl;
        _api = new TestControlClient(apiBaseUrl);
        _productsPage = new ProductsPage(driver, wait);
        _cartPage = new CartPage(driver, wait);
        _checkoutPage = new CheckoutPage(driver, wait);
    }

    public async Task PreconditionsAsync(CancellationToken cancellationToken = default)
    {
        LogInfo("Preconditions: services healthy, baseline reset, and checkout reachable with item in cart.");
        await _api.ResetBaselineAsync(cancellationToken);
        LogInfo("Preconditions completed.");
    }

    public void Step1_OpenCheckoutState()
    {
        LogInfo("Step 1 — Open checkout state. Expected: checkout page is loaded.");

        _productsPage.Open(_baseUrl);
        _productsPage.WaitUntilLoaded();
        _productsPage.AddToCart("HOODIE-CLASSIC");

        _cartPage.WaitUntilLoaded();
        _cartPage.GoToCheckout();

        _checkoutPage.WaitUntilLoaded();
        AssertUrlContains(_driver, "/checkout");

        LogInfo("Step 1 completed: checkout page loaded.");
    }

    public void Step2_SubmitInvalidDetails()
    {
        LogInfo("Step 2 — Submit invalid details. Expected: checkout submission is blocked.");

        _checkoutPage.FillInvalidDetailsAndSubmit();

        AssertUrlContains(_driver, "/checkout");
        Assert.DoesNotContain("/checkout/confirmation/", _driver.Url, StringComparison.OrdinalIgnoreCase);

        LogInfo("Step 2 completed: invalid submission did not complete checkout.");
    }

    public void Step3_ValidateErrorFeedbackAndRoute()
    {
        LogInfo("Step 3 — Validate error feedback and route. Expected: user remains on checkout and sees validation feedback.");

        AssertUrlContains(_driver, "/checkout");
        Assert.True(_checkoutPage.ShowsValidationFeedback());

        LogInfo("Step 3 completed: validation feedback is visible on checkout page.");
    }

    public void ValidateExpectedResults()
    {
        LogInfo("Validating expected results: invalid checkout blocked and validation feedback displayed.");
        AssertUrlContains(_driver, "/checkout");
        Assert.DoesNotContain("/checkout/confirmation/", _driver.Url, StringComparison.OrdinalIgnoreCase);
        Assert.True(_checkoutPage.ShowsValidationFeedback());
        LogInfo("Expected results validated successfully.");
    }
}
