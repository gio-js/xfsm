using System.Globalization;
using System.Reflection;

namespace Xfsm.SqlServer.Test.Utils
{
    public static class StringExtensions
    {
        public static DateTime ToDateTime(this string stringifiedDateTime)
        {
            return DateTime.ParseExact(stringifiedDateTime, "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
        }

        public static string AsResourceString(this string resource)
        {
            Assembly libAssembly = Assembly.GetAssembly(typeof(XfsmDatabaseProvider));
            using Stream stream = libAssembly.GetManifestResourceStream($"Xfsm.SqlServer.{resource}");
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
