using Ioc.ObjModels.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IocContainer.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var menus = MyMenus();
            ViewBag.Menus = menus;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public List<MenuMaster> MyMenus()
        {
            return new List<MenuMaster> {
            new MenuMaster { MenuId  =1, MenuText="Home", ParentId = 0, ControllerName="Home", ActionName = "Index" },
            new MenuMaster { MenuId  =2, MenuText="Sales", ParentId = 0, ControllerName="Sales", ActionName = "Sales" },
            new MenuMaster { MenuId  =3, MenuText="Report", ParentId = 0, ControllerName="Report", ActionName = "Report" },
            new MenuMaster { MenuId  =4, MenuText="About Us", ParentId = 1, ControllerName="Home", ActionName = "Index" },
            new MenuMaster { MenuId  =5, MenuText="Company Profile", ParentId = 1, ControllerName="Home", ActionName = "Index" },
            new MenuMaster { MenuId  =6, MenuText="Add Invoice", ParentId = 2, ControllerName="Sale", ActionName = "Sale" },
            new MenuMaster { MenuId  =7, MenuText="Update Invice", ParentId = 2, ControllerName="Home", ActionName = "Index" },
            new MenuMaster { MenuId  =8, MenuText="Delete Invoice", ParentId = 2, ControllerName="Home", ActionName = "Index" },
            new MenuMaster { MenuId  =9, MenuText="Daily Report", ParentId = 3, ControllerName="Home", ActionName = "Index" },
            new MenuMaster { MenuId  =10, MenuText="Monthly Report", ParentId = 3, ControllerName="Home", ActionName = "Index" },
            new MenuMaster { MenuId  =11, MenuText="Update Invice 1", ParentId = 7, ControllerName="Home", ActionName = "Index" },
            new MenuMaster { MenuId  =12, MenuText="Update Invice 2", ParentId = 11, ControllerName="Home", ActionName = "Index" },
            };
        }
    }
}