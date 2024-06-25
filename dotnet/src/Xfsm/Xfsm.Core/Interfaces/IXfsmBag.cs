using System;
using System.IO;
using Xfsm.Core.Abstract;
using Xfsm.Core.Enums;

namespace Xfsm.Core.Interfaces
{
    /// <summary>
    /// Abstract implementation of an Xfsm
    /// </summary>
    public interface IXfsmBag<T>
    {
        /// <summary>
        /// The method will check every initialization on database has been done successfully.
        /// The state machine will run successfully based on data point of view.
        /// </summary>
        void EnsureInitialized();

        /// <summary>
        /// In order to initialize an external data source (statically or dinamically) you can use this method
        /// in order to retrieve the right scripts to run against your database.
        /// </summary>
        /// <returns></returns>
        string RetrieveDDLScript();

        /// <summary>
        /// Returns the state machine peeking mode (basically a queue or a stack)
        /// </summary>
        /// <returns></returns>
        XfsmPeekMode GetPeekMode();

        /// <summary>
        /// Retrieve a single element from the bag (if any) using the fetch mode
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        IXfsmElement<T> Peek(Enum state);

        /// Deletes every element from the bag
        /// </summary>
        /// <returns></returns>
        void Clear();

        /// <summary>
        /// Sets the element in an error state
        /// </summary>
        /// <returns></returns>
        void Error(IXfsmElement<T> element, string errorMessage);

        /// <summary>
        /// Sets the element an processed in done state
        /// </summary>
        /// <returns></returns>
        void Done(IXfsmElement<T> element);

        /// <summary>
        /// Add a new element to the bag
        /// </summary>
        /// <param name="businessElement"></param>
        /// <param name="elementState"></param>
        long AddElement(T businessElement, Enum elementState);
    }
}
