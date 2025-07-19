using Ioc.ObjModels.Model;
using Microsoft.AspNetCore.Mvc;

namespace IocContainer.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SaveStudentWithoutSerialize(Student student)
        {
            return Json("student saved successfully");
        }
    }
}
