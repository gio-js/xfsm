using System;
using System.Threading.Tasks;
using Xfsm.Core.Interfaces;

namespace Xfsm.Core
{
    /// <summary>
    /// XfsmProcessor is in charge of picking elements from the bag and execute the business domain
    /// code related to the state
    /// </summary>
    public class XfsmProcessor<T>
    {
        private readonly IXfsmBag<T> xfsmBag;
        private int maximumElementsToEleborate;
        private TimeSpan maximumTimeOfElaboration;
        private IXfsmState<T> state;
        private IXfsmStateContextFactory<T> stateContextFactory;

        public XfsmProcessor(IXfsmBag<T> xfsmBag, IXfsmState<T> state, int maximumElementsToEleborate, TimeSpan maximumTimeOfElaboration)
            : this(xfsmBag, state, new XfsmStateContextFactory<T>(xfsmBag, state), maximumElementsToEleborate, maximumTimeOfElaboration)
        { }

        public XfsmProcessor(IXfsmBag<T> xfsmBag, IXfsmState<T> state)
            : this(xfsmBag, state, new XfsmStateContextFactory<T>(xfsmBag, state), 10, TimeSpan.FromMinutes(10))
        { }

        internal XfsmProcessor(IXfsmBag<T> xfsmBag, IXfsmState<T> state, IXfsmStateContextFactory<T> stateContextFactory, int maximumElementsToEleborate, TimeSpan maximumTimeOfElaboration)
        {
            this.xfsmBag = xfsmBag ?? throw new ArgumentNullException(nameof(xfsmBag));
            this.state = state ?? throw new ArgumentNullException(nameof(state));
            this.stateContextFactory = stateContextFactory ?? throw new ArgumentNullException(nameof(stateContextFactory));
            this.maximumElementsToEleborate = maximumElementsToEleborate;
            this.maximumTimeOfElaboration = maximumTimeOfElaboration;
        }

        /// <summary>
        /// Retrieves the first element having the specified state and process it.
        /// </summary>
        /// <param name="state">The state to look for</param>
        /// <param name="maximumElementsToEleborate">Exit after reaching the amount of element elaborated</param>
        /// <param name="maximumTimeOfElaboration">Exit after reaching the amount of time of elaboration</param>
        public void WaitAndProcessElement()
        {
            DateTime startingTime = DateTime.Now;
            int processedElements = 0;

            do
            {
                var element = xfsmBag.Peek(state.StateEnum());

                // process element
                if (element != null)
                {
                    try
                    {
                        (stateContextFactory.Create(element) as XfsmStateContext<T>).Execute(); // execute the "state" against the element
                    }
                    catch (Exception ex)
                    {
                        this.xfsmBag.Error(element, ex.ToString());
                    }

                    processedElements++; // increase processed count regardless it has been successfully processed or not
                }

                // take a break
                Task.Delay(LittleDelay()).ConfigureAwait(false).GetAwaiter().GetResult();

            } while (KeepAlive(processedElements, startingTime));
        }

        private bool KeepAlive(int processedElements, DateTime startingTime)
        {
            return
                (processedElements < this.maximumElementsToEleborate) && // not enough elements processed
                (DateTime.Now.Subtract(startingTime) < maximumTimeOfElaboration); // not enough time elapsed from start
        }

        private int LittleDelay()
        {
            // TODO: should this become incremental? and maybe decreased after a while
            return 100;
        }
    }
}