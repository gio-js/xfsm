using System;
using System.Collections.Generic;
using System.Text;

namespace Xfsm.Core.Enums
{
    /// <summary>
    /// The xfsm bag can be fetched in following listed modes.
    /// </summary>
    public enum XfsmFetchMode
    {
        /// <summary>
        /// Classic FIFO
        /// </summary>
        Queue,

        /// <summary>
        /// Classic LIFO
        /// </summary>
        Stack
    }
}
