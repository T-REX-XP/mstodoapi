using System;

namespace MSTodoApi.Infrastructure.Utils
{
    public interface IDatetimeUtils
    {
        string FormatLongUtc(DateTime dateTime);
        DateTime GetStartOfDay(DateTime dateTime);
        DateTime GetEndOfDay(DateTime dateTime);
    }
}