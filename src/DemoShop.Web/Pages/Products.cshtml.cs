using System.Linq;
using DemoShop.Web.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoShop.Web.Pages;

public class ProductsModel : PageModel
{
    private readonly AppDbContext _dbContext;

    public ProductsModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IReadOnlyList<ProductViewModel> Products { get; private set; } = Array.Empty<ProductViewModel>();

    public void OnGet()
    {
        Products = _dbContext.Products
            .OrderBy(p => p.Id)
            .Select(p => new ProductViewModel(p.Id, p.Name, p.Price))
            .ToList();
    }

    public record ProductViewModel(int Id, string Name, decimal Price);
}
