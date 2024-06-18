using System;
using Xfsm.Core.Abstract;

namespace Xfsm.Core
{
    /// <summary>
    /// XfsmDataRolling deletes elaborated elements by specified threshold
    /// </summary>
    public class XfsmDataRolling<T>
    {
        private readonly XfsmBag<T> xfsmInstance;

        protected XfsmDataRolling(XfsmBag<T> xfsmInstance)
        {
            this.xfsmInstance = xfsmInstance;
        }

        /// <summary>
        /// Execute data rolling on elaborated elements
        /// </summary>
        public void ExecuteRolling()
        {
            throw new NotImplementedException();
        }

    }
}