using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoShop.Web.Pages;

public class ProductsModel : PageModel
{
    private static readonly IReadOnlyList<ProductViewModel> SeedProducts = new List<ProductViewModel>
    {
        new(1, "Essential Hoodie", 48.00m),
        new(2, "Canvas Tote", 22.50m),
        new(3, "Ceramic Mug", 14.00m),
        new(4, "Notebook Set", 18.75m),
        new(5, "Wireless Charger", 36.40m)
    };

    public IReadOnlyList<ProductViewModel> Products { get; private set; } = Array.Empty<ProductViewModel>();

    public void OnGet()
    {
        Products = SeedProducts;
    }

    public record ProductViewModel(int Id, string Name, decimal Price);
}
