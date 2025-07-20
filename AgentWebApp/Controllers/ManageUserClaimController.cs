using Microsoft.AspNetCore.Mvc;

namespace AgentWebApp.Controllers
{
    public class ManageUserClaimController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
