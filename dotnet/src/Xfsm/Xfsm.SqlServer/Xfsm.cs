using System;
using System.IO;
using System.Reflection;
using Xfsm.Core.Enums;
using Xfsm.Core.Interfaces;
using Xfsm.SqlServer.Internal;

namespace Xfsm.SqlServer
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Xfsm<T> : Core.Abstract.Xfsm<T>
    {
        private readonly IXfsmState initialState;
        private readonly IXfsmState endingState;
        private readonly Core.Abstract.XfsmDatabaseProvider databaseProvider;
        private readonly XfsmFetchMode fetchMode;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="initialState"></param>
        /// <param name="endingState"></param>
        /// <param name="databaseProvider"></param>
        /// <param name="fetchMode"></param>
        public Xfsm(IXfsmState initialState, IXfsmState endingState, Core.Abstract.XfsmDatabaseProvider databaseProvider, XfsmFetchMode fetchMode) : base(initialState, endingState, databaseProvider, fetchMode)
        {
            this.initialState = initialState ?? throw new ArgumentNullException(nameof(initialState));
            this.endingState = endingState ?? throw new ArgumentNullException(nameof(endingState));
            this.databaseProvider = databaseProvider ?? throw new ArgumentNullException(nameof(databaseProvider));
            this.fetchMode = fetchMode;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="businessElement"></param>
        /// <param name="elementState"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void AddElement(T businessElement, IXfsmState elementState)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override void EnsureInitialized()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="fetchState"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override IXfsmElement<T> Fetch(IXfsmState fetchState)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override string RetrieveDDLScript()
        {
            Assembly libAssembly = Assembly.GetAssembly(typeof(XfsmDatabaseProvider));
            using Stream stream = libAssembly.GetManifestResourceStream($"Xfsm.SqlServer.Scripts.DataModel.sql");
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}