using Ioc.ObjModels.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;

namespace IocContainer.Controllers
{
    public class TImeZoneSetController : Controller
    {
        public IActionResult Index()
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            ReadOnlyCollection<TimeZoneInfo> zones = TimeZoneInfo.GetSystemTimeZones();
            foreach (TimeZoneInfo zone in zones)
            {
                map.Add(zone.DisplayName + " second; " + zone.BaseUtcOffset.TotalSeconds, zone.Id);
            }
            ViewBag.timezone = map;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TimeZoneModel tz)
        {
            var zone = tz.TimeZoneName;
            ISession session = HttpContext.Session;
            session.SetString("timezone", zone);
            return RedirectToAction("Index");
        }
    }
}
