using NodaTime.TimeZones;
using System.Text;

namespace GoogleCalendar_App.Common
{
    public static class Method
    {
        public static string urlEncodeForGoogle(string url)
        {
            string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-.~";
            StringBuilder result = new StringBuilder();
            foreach (char symbol in url)
            {
                if (unreservedChars.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else
                {
                    result.Append("%" + ((int)symbol).ToString("X2"));
                }
            }

            return result.ToString();

        }

        public static string WindowsToIana(string windowsTimeZoneId)
        {
            if (windowsTimeZoneId.Equals("UTC", StringComparison.Ordinal))
                return "Etc/UTC";

            var tzdbSource = TzdbDateTimeZoneSource.Default;
            var windowsMapping = tzdbSource.WindowsMapping.PrimaryMapping
                .FirstOrDefault(mapping => mapping.Key.Equals(windowsTimeZoneId, StringComparison.OrdinalIgnoreCase));

            return windowsMapping.Value;
        }
    }
}
