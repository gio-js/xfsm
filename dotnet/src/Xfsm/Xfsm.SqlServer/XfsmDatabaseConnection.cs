using Microsoft.Data.SqlClient;
using Xfsm.Core.Interfaces;

namespace Xfsm.SqlServer
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class XfsmDatabaseConnection : IXfsmDatabaseConnection
    {
        private readonly string connectionString;
        private SqlConnection connection;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public XfsmDatabaseConnection(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Commit()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Execute(string sqlStatement)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public T Query<T>(string sqlQuery)
        {
            if (this.connection == null)
            {
                this.connection = new SqlConnection(connectionString);
                this.connection.Open();
            }

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = sqlQuery;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    return (T)reader.GetValue(0);
                }
            }
        }
    }
}
