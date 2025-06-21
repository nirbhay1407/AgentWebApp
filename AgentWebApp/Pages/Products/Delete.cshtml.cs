using AgentWebApp.Models;
using AgentWebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AgentWebApp.Pages.Products;

public class DeleteModel : PageModel
{
    private readonly IProductRepository _productRepository;

    public DeleteModel(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [BindProperty]
    public Product Product { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _productRepository.GetByIdAsync(id.Value);
        if (product == null)
        {
            return NotFound();
        }

        Product = product;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        await _productRepository.DeleteAsync(id.Value);
        return RedirectToPage("./Index");
    }
}