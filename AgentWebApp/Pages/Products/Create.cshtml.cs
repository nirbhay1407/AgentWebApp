using AgentWebApp.Models;
using AgentWebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AgentWebApp.Pages.Products;

public class CreateModel : PageModel
{
    private readonly IProductRepository _productRepository;

    public CreateModel(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [BindProperty]
    public Product Product { get; set; } = default!;

    public void OnGet()
    {
        Product = new Product { Status = true };
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _productRepository.AddAsync(Product);
        return RedirectToPage("./Index");
    }
}