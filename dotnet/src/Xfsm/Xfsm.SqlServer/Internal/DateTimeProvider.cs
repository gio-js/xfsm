using System;

namespace Xfsm.SqlServer.Internal
{
    /// <summary>
    /// Useful class used to "setup" date/time for testing purpose
    /// </summary>
    internal static class DateTimeProvider
    {
        private static DateTimeOffset? instance;
        public static void Set(DateTimeOffset date)
        {
            instance = date;
        }

        public static DateTimeOffset Now()
        {
            if (instance.HasValue)
                return instance.Value;

            return DateTimeOffset.Now;
        }
    }
}
