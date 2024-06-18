using System;
using Xfsm.Core.Interfaces;

namespace Xfsm.Core
{
    /// <summary>
    /// State context represents the actor in charge of execution of business logic
    /// </summary>
    public class XfsmStateContextFactory<T> : IXfsmStateContextFactory<T>
    {
        private IXfsmBag<T> xfsmBag;
        private IXfsmState<T> state;

        public XfsmStateContextFactory(IXfsmBag<T> xfsmBag, IXfsmState<T> state) { }

        /// <summary>
        /// Executes the related business logic of current state
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public IXfsmStateContext Create(IXfsmElement<T> element)
        {
            return new XfsmStateContext<T>(xfsmBag, state, element);
        }
    }
}
