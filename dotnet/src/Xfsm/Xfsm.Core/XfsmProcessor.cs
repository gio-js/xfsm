using System;
using System.Threading.Tasks;
using Xfsm.Core.Abstract;
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
        private XfsmDatabaseProvider provider;
        private int elaboratedElements;
        private TimeSpan startTimespan;
        private IXfsmState<T> state;
        private IXfsmStateContextFactory<T> stateContextFactory;

        public XfsmProcessor(IXfsmBag<T> xfsmBag, IXfsmState<T> state, int maximumElementsToEleborate, TimeSpan maximumTimeOfElaboration)
            : this(xfsmBag, new XfsmStateContextFactory<T>(xfsmBag, state), maximumElementsToEleborate, maximumTimeOfElaboration)
        { }

        public XfsmProcessor(IXfsmBag<T> xfsmBag, IXfsmState<T> state)
            : this(xfsmBag, new XfsmStateContextFactory<T>(xfsmBag, state), 10, TimeSpan.FromMinutes(10))
        { }

        internal XfsmProcessor(IXfsmBag<T> xfsmBag, IXfsmStateContextFactory<T> stateContextFactory, int maximumElementsToEleborate, TimeSpan maximumTimeOfElaboration)
        {
            this.xfsmBag = xfsmBag ?? throw new ArgumentNullException(nameof(xfsmBag));
        }

        /// <summary>
        /// Retrieves the first element having the specified state and process it.
        /// </summary>
        /// <param name="state">The state to look for</param>
        /// <param name="maximumElementsToEleborate">Exit after reaching the amount of element elaborated</param>
        /// <param name="maximumTimeOfElaboration">Exit after reaching the amount of time of elaboration</param>
        public void WaitAndProcessElement()
        {
            do
            {
                var element = xfsmBag.Peek(state.StateEnum());

                try
                {
                    if (element != null)
                    {
                        var context = stateContextFactory.Create(element) as XfsmStateContext<T>;
                        context.Execute();
                    }

                    // take a break
                    Task.Delay(IncrementalDelay()).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    this.xfsmBag.Error(element, ex.ToString());
                }



            } while (KeepAlive());
        }

        private bool KeepAlive()
        {
            return true;
        }

        private int IncrementalDelay()
        {
            return 100;
        }
    }
}