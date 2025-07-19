using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeZoneSetController : ControllerBase
    {
        public TimeZoneSetController()
        {

        }

        [HttpGet]
        public Dictionary<string, object> Get()
        {
            /*ISession session = HttpContext.Session;
            string ss = session.GetString("timezone");*/
            DateTime v = DateTime.Now;
            Dictionary<string, object> map = new Dictionary<string, object>();
            ReadOnlyCollection<TimeZoneInfo> zones = TimeZoneInfo.GetSystemTimeZones();
            foreach (TimeZoneInfo zone in zones)
            {
                /*if (zone.IsDaylightSavingTime)
                {

                }*/
                map.Add(zone.DisplayName + " second; " + zone.BaseUtcOffset.TotalSeconds, zone.Id);
            }
            /*string offset = "+9:00";
            offset.Split(':');
            v = v.AddHours(Convert.ToDouble(offset.Split(':')[0]));*/
            return map;
        }
    }
}
