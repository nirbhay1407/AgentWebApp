using AgentWebApp.Models;
using AgentWebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AgentWebApp.Pages.Products;

public class DetailsModel : PageModel
{
    private readonly IProductRepository _productRepository;

    public DetailsModel(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Product? Product { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Product = await _productRepository.GetByIdAsync(id);
        if (Product == null)
        {
            return NotFound();
        }
        return Page();
    }
}
