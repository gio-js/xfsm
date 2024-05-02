using System;

namespace Xfsm.Core.Interfaces
{
    /// <summary>
    /// Represents a single specific elements in the xfsm bag
    /// </summary>
    public interface IXfsmElement<T>
    {
        /// <summary>
        /// The element current state
        /// </summary>
        /// <returns></returns>
        Enum GetState();

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
        DateTimeOffset GetPeekTimestamp();

        /// <summary>
        /// The business element representation
        /// </summary>
        /// <returns></returns>
        T GetBusinessElement();
    }
}
