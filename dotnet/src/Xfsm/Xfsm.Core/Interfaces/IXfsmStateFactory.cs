using System;

namespace Xfsm.Core.Interfaces
{
    /// <summary>
    /// Create the state factory
    /// </summary>
    public interface IXfsmStateFactory
    {
        /// <summary>
        /// Creates the state instance executor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        IXfsmState<T> Create<T>(Enum state);
    }
}
