using Microsoft.AspNetCore.Mvc;

namespace IocContainer.Controllers
{
    public class UserClaimController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
