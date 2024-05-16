using System;
using Xfsm.Core.Enums;

namespace Xfsm.Core.Interfaces
{
    /// <summary>
    /// Represents a single specific elements in the xfsm bag
    /// </summary>
    public interface IXfsmElement<T>
    {
        /// <summary>
        /// The bag unique element id
        /// </summary>
        /// <returns></returns>
        long GetId();

        /// <summary>
        /// The element current state
        /// </summary>
        /// <returns></returns>
        int GetState();

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
        DateTimeOffset? GetPeekedTimestamp();

        /// <summary>
        /// The business element representation
        /// </summary>
        /// <returns></returns>
        T GetBusinessElement();

        /// <summary>
        /// Peek current status
        /// </summary>
        XfsmPeekStatus GetPeekStatus();

        /// <summary>
        /// Returns the processing error registered on business logic
        /// </summary>
        string GetError();
    }
}
