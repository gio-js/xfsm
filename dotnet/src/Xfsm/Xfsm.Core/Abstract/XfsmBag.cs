using System;
using Xfsm.Core.Enums;
using Xfsm.Core.Interfaces;

namespace Xfsm.Core.Abstract
{
    /// <summary>
    /// Abstract implementation of an Xfsm
    /// </summary>
    public abstract class XfsmBag<T>
    {
        protected readonly XfsmDatabaseProvider databaseProvider;
        protected readonly XfsmPeekMode peekMode;

        /// <summary>
        /// The base xTended Finite State Machine constructor
        /// </summary>
        /// <param name="databaseProvider"></param>
        /// <param name="fetchMode"></param>
        public XfsmBag(XfsmDatabaseProvider databaseProvider, XfsmPeekMode fetchMode)
        {
            this.databaseProvider = databaseProvider ?? throw new ArgumentNullException(nameof(databaseProvider));
            this.peekMode = fetchMode;
        }

        /// <summary>
        /// The method will check every initialization on database has been done successfully.
        /// The state machine will run successfully based on data point of view.
        /// </summary>
        public abstract void EnsureInitialized();

        /// <summary>
        /// In order to initialize an external data source (statically or dinamically) you can use this method
        /// in order to retrieve the right scripts to run against your database.
        /// </summary>
        /// <returns></returns>
        public abstract string RetrieveDDLScript();

        /// <summary>
        /// Returns the state machine peeking mode (basically a queue or a stack)
        /// </summary>
        /// <returns></returns>
        public XfsmPeekMode GetPeekMode()
        {
            return peekMode;
        }

        /// <summary>
        /// Retrieve a single element from the bag (if any) using the fetch mode
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public abstract XfsmElement<T> Peek(Enum state);

        /// <summary>
        /// Add a new element to the bag
        /// </summary>
        /// <param name="businessElement"></param>
        /// <param name="elementState"></param>
        public abstract long AddElement(T businessElement, Enum elementState);
    }
}
