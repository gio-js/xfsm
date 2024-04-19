using Xfsm.Core.Base;

namespace Xfsm.Core.Interfaces
{
    /// <summary>
    /// Represents the element state
    /// </summary>
    public interface IXfsmState
    {
        /// <summary>
        /// Execute the state releated business code
        /// </summary>
        void Execute();

        /// <summary>
        /// The unique index representation of the state inside the state machine
        /// </summary>
        /// <returns></returns>
        int GetStateUniqueIndex();

        /// <summary>
        /// The current state context
        /// </summary>
        /// <returns></returns>
        XfsmStateContext GetContext();
    }
}
