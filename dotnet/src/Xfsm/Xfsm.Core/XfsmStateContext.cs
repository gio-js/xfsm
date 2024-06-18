using System;
using Xfsm.Core.Interfaces;

namespace Xfsm.Core
{
    /// <summary>
    /// State context represents the actor in charge of execution of business logic
    /// </summary>
    public class XfsmStateContext<T> : IXfsmStateContext
    {
        private IXfsmBag<T> xfsmInstance;
        private IXfsmState<T> state;
        private IXfsmElement<T> element;

        /// <summary>
        /// Creates the state context
        /// </summary>
        /// <param name="element"></param>
        public XfsmStateContext(IXfsmBag<T> xfsmInstance, IXfsmState<T> state, IXfsmElement<T> element)
        {
            this.xfsmInstance = xfsmInstance;
            this.element = element;
        }

        /// <summary>
        /// Executes the related business logic of current state
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        internal void Execute()
        {
            state.Execute(element.GetBusinessElement(), this);
        }

        /// <summary>
        /// Changes the state of specified element
        /// </summary>
        /// <param name="state"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void ChangeState(Enum state)
        {
            // TODO: should be transactional
            xfsmInstance.Done(element);
            xfsmInstance.AddElement(element.GetBusinessElement(), state);
        }
    }
}
