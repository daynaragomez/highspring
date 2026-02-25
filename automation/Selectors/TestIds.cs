namespace Highspring.Automation.Selectors;

public static class TestIds
{
    public const string PageHome = "page-home";
    public const string PageProducts = "page-products";
    public const string PageCart = "page-cart";
    public const string PageCheckout = "page-checkout";
    public const string PageCheckoutConfirmation = "page-checkout-confirmation";

    public const string ApplyDiscountInput = "apply-discount-input";
    public const string ApplyDiscountButton = "apply-discount-button";

    public const string SubtotalValue = "subtotal-value";
    public const string DiscountTotal = "discount-total";
    public const string TaxTotal = "tax-total";
    public const string OrderTotal = "order-total";

    public const string CheckoutSubmit = "checkout-submit";
    public const string OrderConfirmationId = "order-confirmation-id";

    public static string AddToCart(string sku) => $"add-to-cart-{sku}";
    public static string QuantityInput(string sku) => $"qty-input-{sku}";
}
