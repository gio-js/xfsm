using System;
using Xfsm.Core.Enums;
using Xfsm.Core.Interfaces;

namespace Xfsm.Core.Abstract
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public abstract class XfsmBag<T> : IXfsmBag<T>
    {
        protected readonly XfsmDatabaseProvider databaseProvider;
        protected readonly XfsmPeekMode peekMode;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public XfsmBag(XfsmDatabaseProvider databaseProvider, XfsmPeekMode peekMode)
        {
            this.databaseProvider = databaseProvider ?? throw new ArgumentNullException(nameof(databaseProvider));
            this.peekMode = peekMode;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public abstract void EnsureInitialized();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public abstract string RetrieveDDLScript();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public XfsmPeekMode GetPeekMode()
        {
            return peekMode;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public abstract IXfsmElement<T> Peek(Enum state);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public abstract void Error(IXfsmElement<T> element, string errorMessage);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public abstract void Done(IXfsmElement<T> element);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public abstract long AddElement(T businessElement, Enum elementState);
    }
}
