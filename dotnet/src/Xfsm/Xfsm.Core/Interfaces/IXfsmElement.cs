using System;
using System.Collections.Generic;
using System.Text;

namespace Xfsm.Core.Interfaces
{
    /// <summary>
    /// Represents a single specific elements in the xfsm bag
    /// </summary>
    public interface IXfsmElement<T>
    {
        /// <summary>
        /// Element inserted in the bag timestamp
        /// </summary>
        /// <returns></returns>
        DateTimeOffset GetInsertedTimestamp();

        /// <summary>
        /// Element last update timestamp
        /// </summary>
        /// <returns></returns>
        DateTimeOffset GetLastUpdateTimestamp();

        /// <summary>
        /// Element fetch from bag timestamp
        /// </summary>
        /// <returns></returns>
        DateTimeOffset GetFetchTimestamp();

        /// <summary>
        /// The element current state
        /// </summary>
        /// <returns></returns>
        IXfsmState GetState();

        /// <summary>
        /// The business element representation
        /// </summary>
        /// <returns></returns>
        T GetBusinessElement();
    }
}
