using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;

namespace Ioc.Core.Helper
{
    public class LocalDateTimeConverter : ValueConverter<DateTime, DateTime>
    {
        static readonly Expression<Func<DateTime, DateTime>> ConvertTo = x => ConvertToUTCFromFakedTimeZone(x);
        static readonly Expression<Func<DateTime, DateTime>> ConvertFrom = x => ConvertFromUTCToFakedTimeZone(x);

        public LocalDateTimeConverter() : base(ConvertTo, ConvertFrom)
        {
        }
        private static DateTime ConvertToUTCFromFakedTimeZone(DateTime x)
        {
            DateTime v = DateTime.Now;
            string offset = "+9:00";
            offset.Split(':');
            v = v.AddHours(Convert.ToDouble(offset.Split(':')[0]));
            return v;
        }


        private static DateTime ConvertFromUTCToFakedTimeZone(DateTime x)
        {
            DateTime v = DateTime.Now;
            string offset = "+9:00";
            offset.Split(':');
            v = v.AddHours(Convert.ToDouble(offset.Split(':')[0]));
            return v;
        }


    }
}