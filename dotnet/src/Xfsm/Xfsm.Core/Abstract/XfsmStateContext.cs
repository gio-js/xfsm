using System;
using Xfsm.Core.Interfaces;

namespace Xfsm.Core.Abstract
{
    /// <summary>
    /// State context represents the actor in charge of execution of business logic
    /// </summary>
    public abstract class XfsmStateContext<T>
    {
        private XfsmElement<T> element;

        /// <summary>
        /// Creates the state context
        /// </summary>
        /// <param name="element"></param>
        public XfsmStateContext(XfsmDatabaseProvider databaseProvider, IXfsmStateFactory stateFactory, XfsmElement<T> element)
        {
            this.element = element;
        }

        /// <summary>
        /// Executes the related business logic of current state
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Execute()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Changes the state of specified element
        /// </summary>
        /// <param name="state"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void ChangeState(Enum state)
        {
            throw new NotImplementedException();
        }


    }
}
