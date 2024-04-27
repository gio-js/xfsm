using Xfsm.Core.Interfaces;

namespace Xfsm.SqlServer
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class XfsmDatabaseProvider : Xfsm.Core.Abstract.XfsmDatabaseProvider
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="connectionString"></param>
        public XfsmDatabaseProvider(string connectionString) : base(connectionString) { }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override IXfsmDatabaseConnection GetConnection()
        {
            return new XfsmDatabaseConnection(base.connectionString);
        }
    }
}
