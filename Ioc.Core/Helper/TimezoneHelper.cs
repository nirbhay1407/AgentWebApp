using Microsoft.EntityFrameworkCore;
using TimeZoneConverter;

namespace Ioc.Core.Helper
{
    public static class TimezoneHelper
    {
        /*  static readonly Expression<Func<DateTime, DateTime>> ConvertTo = x => ConvertToUTCFromFakedTimeZone(x);
          static readonly Expression<Func<DateTime, DateTime>> ConvertFrom = x => ConvertFromUTCToFakedTimeZone(x);*/
        public static DateTime getLocaltimeFromUniversal(DateTime utcDateTime, string timeZone)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime,
            TimeZoneInfo.FindSystemTimeZoneById(TZConvert.IanaToWindows(timeZone)));
        }
        public static DateTime ConvertLocalToUTCwithTimeZone(DateTime localDateTime, string timezone)
        {
            localDateTime = DateTime.SpecifyKind(localDateTime, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(localDateTime,
              TimeZoneInfo.FindSystemTimeZoneById(TZConvert.IanaToWindows(timezone)));
        }

        public static ModelBuilder DateTimeZoneConvertion(this ModelBuilder modelBuilder)
        {
            var dtConverter = new LocalDateTimeConverter();
            var dtNullableConverter = new LocalNullableDateTimeConverter();

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                        property.SetValueConverter(dtConverter);
                    else if (property.ClrType == typeof(DateTime?))
                        property.SetValueConverter(dtNullableConverter);
                }
            }

            return modelBuilder;

        }
    }






    /*var selectedTimeZone = GetSelectedTimeZoneFromCookie(AppHttpContext.Current);
            return DateTimeZoneProviders.Tzdb[selectedTimeZone];

         var timeZoneCookieValue = httpContext?.Request?.Cookies[_selectedTimeZoneCookieName];

                if (!string.IsNullOrEmpty(timeZoneCookieValue))
                {
                    var projectTimeZone = JsonConvert.DeserializeObject<ProjectTimeZone>(timeZoneCookieValue);
                    return projectTimeZone.Name;
                }
            }
            catch
{
}

return "Etc/UTC";


var projectTimeZone = JsonConvert.DeserializeObject<ProjectTimeZone>(timeZoneCookieValue);
return projectTimeZone.Name;*/
}
