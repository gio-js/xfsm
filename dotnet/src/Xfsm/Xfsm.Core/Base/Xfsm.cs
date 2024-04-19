using System.Collections.Generic;
using Xfsm.Core.Enums;
using Xfsm.Core.Interfaces;

namespace Xfsm.Core.Base
{
    /// <summary>
    /// Abstract implementation of an Xfsm
    /// </summary>
    public abstract class Xfsm<T>
    {
        private readonly IXfsmState initialState;
        private readonly IList<IXfsmState> endingStates;
        private readonly IXfsmDatabaseProvider databaseProvider;
        private readonly XfsmFetchMode fetchMode;

        /// <summary>
        /// The base xTended Finite State Machine constructor
        /// </summary>
        /// <param name="initialState"></param>
        /// <param name="endingState"></param>
        /// <param name="databaseProvider"></param>
        /// <param name="fetchMode"></param>
        public Xfsm(IXfsmState initialState, IXfsmState endingState, IXfsmDatabaseProvider databaseProvider, XfsmFetchMode fetchMode)
        {
            this.initialState = initialState;
            this.endingStates = new List<IXfsmState>() { endingState };
            this.databaseProvider = databaseProvider;
            this.fetchMode = fetchMode;
        }

        /// <summary>
        /// Add some other ending state for the xfsm
        /// </summary>
        /// <param name="endingState"></param>
        public void AddEndingState(IXfsmState endingState)
        {
            this.endingStates.Add(endingState);
        }

        public abstract void EnsureInitialized();

        public abstract string RetrieveDDLScript();

        public XfsmFetchMode GetFetchMode()
        {
            return this.fetchMode;
        }

        public abstract IXfsmElement<T> Fetch(IXfsmState fetchState);

        public abstract void AddElement(T businessElement, IXfsmState elementState);
    }
}
