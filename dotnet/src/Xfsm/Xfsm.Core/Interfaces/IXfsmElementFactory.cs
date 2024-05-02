using System;
using System.Collections.Generic;
using System.Text;

namespace Xfsm.Core.Interfaces
{
    /// <summary>
    /// Create the bag element object
    /// </summary>
    public interface IXfsmElementFactory
    {
        IXfsmElement<T> Create<T>(Enum state, T businessElement, DateTimeOffset instertedTimestamp, DateTimeOffset lastUpdateTimestamp, DateTimeOffset peekTimestamp);
    }
}
