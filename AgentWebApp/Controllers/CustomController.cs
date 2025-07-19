using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IocContainer.Controllers
{
    public class CustomController : Controller
    {
        public IActionResult Index()
        {
            var menus = MyMenus();
            var menuItems = GenerateMenuItems(menus);
            return View(menuItems);
        }

        private Dictionary<MenuMaster, List<MenuMaster>> GenerateMenuItems(List<MenuMaster> allMenus)
        {
            var menuItemsMap = new Dictionary<MenuMaster, List<MenuMaster>>();

            foreach (var menu in allMenus)
            {
                if (menu.ParentId == 0)
                {
                    menuItemsMap.Add(menu, new List<MenuMaster>());
                }
                else
                {
                    var parentMenu = allMenus.FirstOrDefault(m => m.MenuId == menu.ParentId);
                    if (parentMenu != null)
                    {
                        if (!menuItemsMap.ContainsKey(parentMenu))
                        {
                            menuItemsMap.Add(parentMenu, new List<MenuMaster>());
                        }
                        menuItemsMap[parentMenu].Add(menu);
                    }
                }
            }

            return menuItemsMap;
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


    public partial class MenuMaster
    {
        public int MenuId { get; set; }
        public string MenuText { get; set; }
        public string Description { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        public bool IsChecked { get; set; }
        public List<MenuMaster> menus { get; set; }
        public IEnumerable<SelectListItem> users { get; set; }
    }

}
