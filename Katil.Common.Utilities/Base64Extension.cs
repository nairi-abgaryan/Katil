using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Katil.Common.Utilities
{
    public static class Base64Extension
    {
        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static bool IsBase64String(this string base64String)
        {
            if (base64String == null ||
                base64String.Length == 0 ||
                base64String.Length % 4 != 0 ||
                base64String.Contains(" ") ||
                base64String.Contains("\t") ||
                base64String.Contains("\r") ||
                base64String.Contains("\n"))
            {
                return false;
            }

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch
            {
            }

            return false;
        }
    }
}
