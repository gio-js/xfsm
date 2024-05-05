using System;
using System.Collections.Generic;

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
        IList<T> Query<T>(string sqlQuery, params XfsmDatabaseParameter[] parameters);

        /// <summary>
        /// Executes a sql query against the acquired sql connection and returns first element
        /// </summary>
        /// <param name="sqlQuery"></param>
        T QueryFirst<T>(string sqlQuery, params XfsmDatabaseParameter[] parameters);

        /// <summary>
        /// Executes a sql statement against the acquired sql connection
        /// </summary>
        /// <param name="sqlStatement"></param>
        void Execute(string sqlStatement, params XfsmDatabaseParameter[] parameters);

        /// <summary>
        /// Commit the internal transaction
        /// </summary>
        void Commit();
    }

    /// <summary>
    /// Useful class
    /// </summary>
    public class XfsmDatabaseParameter
    {
        public XfsmDatabaseParameter(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
