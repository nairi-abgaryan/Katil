using System;

namespace Katil.Common.Utilities
{
    public static class DateTimeExtensions
    {
        public static DateTime GetCmDateTime(this DateTime dt)
        {
            return Convert.ToDateTime(dt.ToString("yyyy-MM-dd'T'HH:mm:ss.fffZ"));
        }

        public static string ToCmDateTimeString(this DateTime? dt)
        {
            if (!dt.HasValue)
            {
                return string.Empty;
            }

            return dt.Value.ToString("yyyy-MM-dd'T'HH:mm:ss.fffZ");
        }

        public static string ToCmDateTimeString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd'T'HH:mm:ss.fffZ");
        }
    }
}
