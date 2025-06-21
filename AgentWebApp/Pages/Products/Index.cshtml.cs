using AgentWebApp.Models;
using AgentWebApp.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AgentWebApp.Pages.Products;

public class IndexModel : PageModel
{
    private readonly IProductRepository _productRepository;

    public IndexModel(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public IEnumerable<Product> Products { get; set; } = new List<Product>();

    public async Task OnGetAsync()
    {
        Products = await _productRepository.GetAllAsync();
    }
}