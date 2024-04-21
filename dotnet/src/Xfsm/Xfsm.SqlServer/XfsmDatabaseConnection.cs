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

        public XfsmDatabaseConnection(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Commit()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public T Execute<T>(string sqlStatement)
        {
            if (this.connection == null)
            {
                this.connection = new SqlConnection(connectionString);
                this.connection.Open();
            }

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = sqlStatement;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    return (T)reader.GetValue(0);
                }
            }
        }
    }
}
