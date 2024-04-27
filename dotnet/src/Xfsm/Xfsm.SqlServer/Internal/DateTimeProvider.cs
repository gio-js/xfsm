using System;
using System.Collections.Generic;
using System.Text;

namespace Xfsm.SqlServer.Internal
{
    /// <summary>
    /// Useful class used to 
    /// </summary>
    internal class DateTimeProvider
    {
        public DateTimeOffset Now()
        {
            return DateTimeOffset.Now;
        }
    }
}
