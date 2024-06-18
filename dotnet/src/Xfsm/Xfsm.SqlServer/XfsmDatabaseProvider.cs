using Xfsm.Core.Interfaces;

namespace Xfsm.SqlServer
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    internal class XfsmDatabaseProvider : Xfsm.Core.Abstract.XfsmDatabaseProvider
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="connectionString"></param>
        public XfsmDatabaseProvider(string connectionString) : base(connectionString) { }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        internal override IXfsmDatabaseConnection GetConnection()
        {
            return new XfsmDatabaseConnection(base.connectionString);
        }
    }
}
