using System;
using System.Collections.Generic;
using System.Text;
using Xfsm.Core.Interfaces;

namespace Xfsm.Core.Base
{
    /// <summary>
    /// The state context
    /// </summary>
    public abstract class XfsmStateContext
    {
        /// <summary>
        /// Holds the current state in the Xfsm context
        /// </summary>
        private IXfsmState currentState;

        /// <summary>
        /// The state context holds the sate itself
        /// </summary>
        /// <param name="initialState"></param>
        public XfsmStateContext(IXfsmState initialState)
        {
            this.currentState = initialState;
        }

        /// <summary>
        /// Specific implementation will derive and add the customied behaviour (es. change database element status)
        /// </summary>
        /// <param name="state"></param>
        public abstract void ChangeStateInternal(IXfsmState state);

        /// <summary>
        /// Changes the xfsm contex state
        /// </summary>
        /// <param name="state"></param>
        public void ChangeState(IXfsmState state)
        {
            this.ChangeStateInternal(state);
            this.currentState = state;
        }
    }
}
