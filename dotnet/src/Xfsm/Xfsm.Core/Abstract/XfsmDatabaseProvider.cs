using Xfsm.Core.Interfaces;

namespace Xfsm.Core.Abstract
{
    /// <summary>
    /// The provider in charge of creating the required database connection
    /// Derived class will be specialized for the required database engines
    /// </summary>
    public abstract class XfsmDatabaseProvider
    {
        protected readonly string connectionString;
        public XfsmDatabaseProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Creates the database connection
        /// </summary>
        /// <returns></returns>
        public abstract IXfsmDatabaseConnection GetConnection();
    }
}
