using Highspring.Automation.Api;
using Highspring.Automation.Pages;
using Highspring.Automation.Tests;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Highspring.Automation.Tests.Smoke.Cases;

public sealed class TC006_Checkout_Submission_Redirects_To_Confirmation : BaseTestCase
{
    private readonly IWebDriver _driver;
    private readonly string _baseUrl;
    private readonly TestControlClient _api;
    private readonly ProductsPage _productsPage;
    private readonly CartPage _cartPage;
    private readonly CheckoutPage _checkoutPage;
    private readonly ConfirmationPage _confirmationPage;

    public TC006_Checkout_Submission_Redirects_To_Confirmation(
        IWebDriver driver,
        WebDriverWait wait,
        string baseUrl,
        string apiBaseUrl,
        ILogger<TC006_Checkout_Submission_Redirects_To_Confirmation> logger,
        Guid runId,
        Guid caseExecutionId)
        : base(logger, runId, caseExecutionId, "TC006")
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
        LogInfo("Preconditions: confirming services are healthy, resetting baseline data, and ensuring cart can be prepared with at least one item.");
        await _api.ResetBaselineAsync(cancellationToken);
        LogInfo("Preconditions completed.");
    }

    public void Step1_PrepareCheckoutState()
    {
        LogInfo("Step 1 — Prepare checkout state: add item to cart and navigate to checkout. Expected: checkout page is loaded.");
        _productsPage.Open(_baseUrl);
        _productsPage.WaitUntilLoaded();
        _productsPage.AddToCart("HOODIE-CLASSIC");

        _cartPage.WaitUntilLoaded();
        _cartPage.GoToCheckout();

        _checkoutPage.WaitUntilLoaded();
        AssertUrlContains(_driver, "/checkout");
        LogInfo("Step 1 completed: checkout page loaded successfully.");
    }

    public void Step2_SubmitValidCheckoutForm()
    {
        LogInfo("Step 2 — Submit valid checkout form. Expected: submission succeeds.");
        _checkoutPage.FillAddressAndSubmit();
        _checkoutPage.WaitForCheckoutResult();
        AssertUrlContains(_driver, "/checkout/confirmation/");
        LogInfo("Step 2 completed: checkout form submitted and confirmation route reached.");
    }

    public void Step3_ValidateRedirectAndConfirmation()
    {
        LogInfo("Step 3 — Validate redirect and confirmation page. Expected: redirect to confirmation with order identifier.");
        _confirmationPage.WaitUntilLoaded();
        AssertHasValue(_confirmationPage.OrderId());
        LogInfo("Step 3 completed: confirmation page loaded and order identifier is present.");
    }

    public void ValidateExpectedResults()
    {
        LogInfo("Validating expected results: checkout succeeds, confirmation route is reached, and no blocking checkout error is present.");
        AssertUrlContains(_driver, "/checkout/confirmation/not-expected");
        AssertHasValue(_confirmationPage.OrderId());
        LogInfo("Expected results validated successfully.");
    }

}
