using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Ioc.Core.Helper
{
    public class LocalNullableDateTimeConverter : ValueConverter<DateTime, DateTime>
    {
        static readonly Expression<Func<DateTime, DateTime>> ConvertTo = x => ConvertToUTCFromFakedTimeZone(x);
        static readonly Expression<Func<DateTime, DateTime>> ConvertFrom = x => ConvertFromUTCToFakedTimeZone(x);
        public LocalNullableDateTimeConverter() : base(ConvertTo, ConvertFrom)
        {
        }


        private static DateTime ConvertFromUTCToFakedTimeZone(DateTime x)
        {
            // TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("+9:00"); 
            //TimeZoneInfo timeInfo = TimeZoneInfo.GetSystemTimeZones()[];
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
            string offset = "+9:00";
            offset.Split(':');
            v = v.AddHours(Convert.ToDouble(offset.Split(':')[0]));
            return v;
        }

        private static DateTime ConvertToUTCFromFakedTimeZone(DateTime x)
        {
            /*TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("+9:00");
            return TimeZoneInfo.ConvertTimeFromUtc(x, timeInfo);*/
            return x;
        }
    }
}