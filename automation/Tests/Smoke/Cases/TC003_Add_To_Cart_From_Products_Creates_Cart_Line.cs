using Highspring.Automation.Api;
using Highspring.Automation.Pages;
using Highspring.Automation.Tests;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace Highspring.Automation.Tests.Smoke.Cases;

public sealed class TC003_Add_To_Cart_From_Products_Creates_Cart_Line : BaseTestCase
{
    private readonly IWebDriver _driver;
    private readonly string _baseUrl;
    private readonly TestControlClient _api;
    private readonly ProductsPage _productsPage;
    private readonly CartPage _cartPage;

    public TC003_Add_To_Cart_From_Products_Creates_Cart_Line(
        IWebDriver driver,
        WebDriverWait wait,
        string baseUrl,
        string apiBaseUrl,
        ILogger<TC003_Add_To_Cart_From_Products_Creates_Cart_Line> logger,
        Guid runId,
        Guid caseExecutionId)
        : base(logger, runId, caseExecutionId, "TC003")
    {
        _driver = driver;
        _baseUrl = baseUrl;
        _api = new TestControlClient(apiBaseUrl);
        _productsPage = new ProductsPage(driver, wait);
        _cartPage = new CartPage(driver, wait);
    }

    public async Task PreconditionsAsync(CancellationToken cancellationToken = default)
    {
        LogInfo("Preconditions: services healthy, baseline reset, clean session, and target product available.");
        await _api.ResetBaselineAsync(cancellationToken);
        LogInfo("Preconditions completed.");
    }

    public void Step1_OpenProductsPage()
    {
        LogInfo("Step 1 — Open products page. Expected: products page is displayed and product cards are visible.");

        _productsPage.Open(_baseUrl);
        _productsPage.WaitUntilLoaded();
        AssertUrlContains(_driver, "/products");

        LogInfo("Step 1 completed: products page loaded.");
    }

    public void Step2_AddProductToCart()
    {
        LogInfo("Step 2 — Add product to cart. Expected: add-to-cart action starts shopping flow.");

        _productsPage.AddToCart("HOODIE-CLASSIC");
        _cartPage.WaitUntilLoaded();
        AssertUrlContains(_driver, "/cart");

        LogInfo("Step 2 completed: add-to-cart action navigated to cart.");
    }

    public void Step3_ValidateCartLineAndTotals()
    {
        LogInfo("Step 3 — Validate cart line and totals. Expected: line exists with quantity 1 and totals are coherent.");

        Assert.Equal(1, _cartPage.Quantity("HOODIE-CLASSIC"));

        var subtotal = _cartPage.Subtotal();
        var discount = _cartPage.Discount();
        var tax = _cartPage.Tax();
        var total = _cartPage.Total();

        Assert.True(subtotal > 0m);
        Assert.Equal(subtotal - discount + tax, total);

        LogInfo("Step 3 completed: cart line exists and totals are coherent.");
    }

    public void ValidateExpectedResults()
    {
        LogInfo("Validating expected results: cart line created with quantity 1 and shopping state is valid.");

        Assert.Equal(1, _cartPage.Quantity("HOODIE-CLASSIC"));
        Assert.Equal(_cartPage.Subtotal() - _cartPage.Discount() + _cartPage.Tax(), _cartPage.Total());

        LogInfo("Expected results validated successfully.");
    }
}
