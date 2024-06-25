using System;
using Xfsm.Core.Interfaces;

namespace Xfsm.Core
{
    /// <summary>
    /// XfsmAppender is in charge of adding new elements to the bag
    /// </summary>
    public class XfsmAppender<T>
    {
        private readonly IXfsmBag<T> xfsmInstance;

        public XfsmAppender(IXfsmBag<T> xfsmInstance)
        {
            this.xfsmInstance = xfsmInstance ?? throw new ArgumentNullException(nameof(xfsmInstance));
        }

        /// <summary>
        /// Adds new element to the bag
        /// </summary>
        /// <param name="businessElement"></param>
        /// <param name="state"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Add(T businessElement, Enum state)
        {
            this.xfsmInstance.AddElement(businessElement, state);
        }

    }
}