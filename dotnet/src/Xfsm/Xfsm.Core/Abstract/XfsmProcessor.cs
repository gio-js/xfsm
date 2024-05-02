using System;

namespace Xfsm.Core.Abstract
{
    /// <summary>
    /// XfsmProcessor is in charge of picking elements from the bag and execute the business domain
    /// code related to the state
    /// </summary>
    public abstract class XfsmProcessor<T>
    {
        private readonly XfsmBag<T> xfsmInstance;

        protected XfsmProcessor(XfsmBag<T> xfsmInstance)
        {
            this.xfsmInstance = xfsmInstance;
        }

        /// <summary>
        /// Retrieves the first element having the specified state and process it.
        /// </summary>
        /// <param name="state">The state to look for</param>
        /// <param name="maximumElementsToEleborate">Exit after reaching the amount of element elaborated</param>
        /// <param name="maximumTimeOfElaboration">Exit after reaching the amount of time of elaboration</param>
        public abstract void WaitAndProcessElement(Enum state, int maximumElementsToEleborate, TimeSpan maximumTimeOfElaboration);

        /// <summary>
        /// Execute data rolling on elaborated elements
        /// </summary>
        public abstract void ExecuteRolling();

    }
}