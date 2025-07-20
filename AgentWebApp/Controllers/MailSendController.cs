using Ioc.ObjModels.Model.CommonModel;
using Microsoft.AspNetCore.Mvc;

namespace AgentWebApp.Controllers
{
    public class MailSendController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SendMail(MailContent abc)
        {
            return Json("Ok");
        }
    }

}
