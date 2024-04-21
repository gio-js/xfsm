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
        /// Returns the state machine fetching mode (basically a queue or a stack)
        /// </summary>
        /// <returns></returns>
        public XfsmFetchMode GetFetchMode()
        {
            return this.fetchMode;
        }

        /// <summary>
        /// Retrieve a single element from the bag (if any) using the fetch mode
        /// </summary>
        /// <param name="fetchState"></param>
        /// <returns></returns>
        public abstract IXfsmElement<T> Fetch(IXfsmState fetchState);

        /// <summary>
        /// Add a new element to the bag
        /// </summary>
        /// <param name="businessElement"></param>
        /// <param name="elementState"></param>
        public abstract void AddElement(T businessElement, IXfsmState elementState);
    }
}
