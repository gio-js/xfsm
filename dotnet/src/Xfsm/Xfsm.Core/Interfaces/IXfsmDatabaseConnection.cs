using System;
using System.Collections.Generic;
using System.Text;

namespace Xfsm.Core.Interfaces
{
    /// <summary>
    /// Xfsm database connection representation
    /// </summary>
    public interface IXfsmDatabaseConnection : IDisposable
    {
        /// <summary>
        /// Executes a sql query against the acquired sql connection
        /// </summary>
        /// <param name="sqlQuery"></param>
        T Query<T>(string sqlQuery);

        /// <summary>
        /// Executes a sql statement against the acquired sql connection
        /// </summary>
        /// <param name="sqlStatement"></param>
        void Execute(string sqlStatement);

        /// <summary>
        /// Commit the internal transaction
        /// </summary>
        void Commit();
    }
}
