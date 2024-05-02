using System;
using System.IO;
using System.Reflection;
using Xfsm.Core.Enums;
using Xfsm.Core.Interfaces;

namespace Xfsm.SqlServer
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class XfsmBag<T> : Core.Abstract.XfsmBag<T>
    {
        public XfsmBag(Core.Abstract.XfsmDatabaseProvider databaseProvider, XfsmPeekMode fetchMode) : base(databaseProvider, fetchMode) { }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void AddElement(T businessElement, Enum elementState)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void EnsureInitialized()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override IXfsmElement<T> Peek(Enum state)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override string RetrieveDDLScript()
        {
            Assembly libAssembly = Assembly.GetAssembly(typeof(XfsmDatabaseProvider));
            using Stream stream = libAssembly.GetManifestResourceStream($"Xfsm.SqlServer.Scripts.DataModel.sql");
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}