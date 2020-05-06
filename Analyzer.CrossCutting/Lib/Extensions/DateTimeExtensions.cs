using System;

namespace Analyzer.CrossCutting.Lib.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToFormatDate(this DateTime date)
        {
            return date.ToString("yyyy-MM-ddTHH:mm:ss");
        }
        public static string ToShortFormatDate(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
        public static string ToFormatDate(this DateTime? date)
        {
            return date?.ToString("yyyy-MM-ddTHH:mm:ss");
        }
        public static string ToShortFormatDate(this DateTime? date)
        {
            return date?.ToString("yyyy-MM-dd");
        }
    }
}
