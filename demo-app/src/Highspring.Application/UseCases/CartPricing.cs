namespace DagoShopFlow.Application.UseCases;

public class CartPricing
{
    public decimal Subtotal { get; set; }

    public decimal DiscountTotal { get; set; }

    public decimal TaxTotal { get; set; }

    public decimal GrandTotal { get; set; }

    public List<TaxBreakdownLine> TaxLines { get; set; } = [];
}
