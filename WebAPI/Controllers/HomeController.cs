using Ioc.Core.DbModel.Models;
using Ioc.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ICategoryRepository _repository;

        public HomeController(ICategoryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var category = new Category
            {
                Description = "first category - very cool description",
                Name = "First category"
            };
            await _repository.Create(category);

            var coolestCategory = await _repository.GetCoolestCategory();

            return View();
        }

        /*public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }*/
    }
}
