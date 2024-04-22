using System.Globalization;

namespace Xfsm.SqlServer.Test.Utils
{
    public static class StringExtensions
    {
        public static DateTime ToDateTime(this string stringifiedDateTime)
        {
            return DateTime.ParseExact(stringifiedDateTime, "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
        }
    }
}
