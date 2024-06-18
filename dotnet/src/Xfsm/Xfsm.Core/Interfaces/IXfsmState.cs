using System;

namespace Xfsm.Core.Interfaces
{
    /// <summary>
    /// Represents the element state
    /// </summary>
    public interface IXfsmState<T>
    {
        /// <summary>
        /// Execute the state releated business code
        /// <param name="businessElement"></param>
        /// </summary>
        void Execute(T businessElement, IXfsmStateContext context);

        /// <summary>
        /// The unique enum value representation of the state
        /// </summary>
        /// <returns></returns>
        Enum StateEnum();
    }
}
