using System;
using Xfsm.Core.Interfaces;

namespace Xfsm.Core.Abstract
{
    /// <summary>
    /// State context represents the actor in charge of execution of business logic
    /// </summary>
    public abstract class XfsmStateContextFactory
    {
        /// <summary>
        /// Executes the related business logic of current state
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public XfsmStateContext<T> Create<T>(XfsmDatabaseProvider databaseProvider, IXfsmStateFactory stateFactory, IXfsmElement<T> element)
        {
            throw new NotImplementedException();
        }
    }
}
