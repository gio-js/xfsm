using System;
using Xfsm.Core.Interfaces;

namespace Xfsm.SqlServer
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class XfsmDatabaseProvider : Xfsm.Core.Abstract.XfsmDatabaseProvider
    {
        public XfsmDatabaseProvider(string connectionString) : base(connectionString) { }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override IXfsmDatabaseConnection OpenConnection()
        {
            throw new NotImplementedException();
        }
    }
}
