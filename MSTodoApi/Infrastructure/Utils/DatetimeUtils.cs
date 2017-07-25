using System;

namespace MSTodoApi.Infrastructure.Utils
{
    public class DatetimeUtils : IDatetimeUtils
    {
        public DateTime GetStartOfDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);
        }

        public DateTime GetEndOfDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999);
        }

        public string FormatLongUtc(DateTime dateTime)
        {
            return string.Format("{0:yyyy-MM-ddTHH:mm:ss.FFFZ}", dateTime);
        }
    }
}