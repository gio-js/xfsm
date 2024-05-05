using System;

namespace Xfsm.Core.Abstract
{
    /// <summary>
    /// XfsmAppender is in charge of adding new elements to the bag
    /// </summary>
    public class XfsmAppender<T>
    {
        private readonly XfsmBag<T> xfsmInstance;

        protected XfsmAppender(XfsmBag<T> xfsmInstance)
        {
            this.xfsmInstance = xfsmInstance;
        }

        /// <summary>
        /// Adds new element to the bag
        /// </summary>
        /// <param name="businessElement"></param>
        /// <param name="state"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Add(T businessElement, Enum state)
        {
            throw new NotImplementedException();
        }

    }
}