using System;
using System.Collections.Generic;
using System.Text;

namespace Xfsm.SqlServer.Extensions
{
    internal static class Dates
    {
        public static string ToSql(this DateTimeOffset offset)
        {
            return offset.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz");
        }
    }
}
