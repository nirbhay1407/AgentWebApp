using Microsoft.AspNetCore.Mvc;

namespace AgentWebApp.Controllers
{
    public class UserClaimController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
