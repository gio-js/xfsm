using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private SqlTransaction transaction;

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
            if (this.transaction == null)
            {
                throw new Exception("No transaction available.");
            }

            this.transaction.Commit();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
            if (this.connection != null)
            {
                this.transaction.Dispose();
                this.transaction = null;
                this.connection.Dispose();
                this.connection = null;
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Execute(string sqlStatement)
        {
            this.InitConnectionWithTransactionIfNeeded();

            using SqlCommand command = connection.CreateCommand();
            command.Transaction = this.transaction;
            command.CommandText = sqlStatement;
            command.ExecuteNonQuery();
        }

        private string[] exPrimitiveTypes = new string[] { "DateTime", "String" };
        private Dictionary<Type, PropertyInfo[]> typesPropertiesCache = new Dictionary<Type, PropertyInfo[]>();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IList<T> Query<T>(string sqlQuery)
        {
            this.InitConnectionWithTransactionIfNeeded();

            Type generic = typeof(T); // retrieve the generic T type involved in query

            using SqlCommand command = connection.CreateCommand();
            command.Transaction = this.transaction;
            command.CommandText = sqlQuery;

            using SqlDataReader reader = command.ExecuteReader();
            if (generic.IsPrimitive || exPrimitiveTypes.Contains(generic.Name))
            {
                reader.Read();
                return new List<T> { (T)reader.GetValue(0) };
            }
            else
            {
                IList<T> elements = new List<T>();
                PropertyInfo[] elementProperties = GetElementProperties(generic);

                while (reader.Read())
                {
                    T element = (T)Activator.CreateInstance(generic);
                    for (int i = 0; i < elementProperties.Length; i++)
                    {
                        PropertyInfo propertyInfo = elementProperties[i];
                        propertyInfo.SetValue(element, reader.GetValue(i));
                    }
                    elements.Add(element);
                }
                return elements;
            }
        }

        /// <summary>
        /// Build a connection with explicit declared transaction if null
        /// </summary>
        private void InitConnectionWithTransactionIfNeeded()
        {
            if (this.connection == null)
            {
                // create connection
                this.connection = new SqlConnection(this.connectionString);
                this.connection.Open();

                // starts a new transaction
                this.transaction = this.connection.BeginTransaction();
            }
        }

        /// <summary>
        /// Simple retrieve of object prorperties by reflection
        /// </summary>
        /// <param name="generic"></param>
        /// <returns></returns>
        private PropertyInfo[] GetElementProperties(Type generic)
        {
            if (!typesPropertiesCache.ContainsKey(generic))
                typesPropertiesCache[generic] = generic.GetProperties();

            return typesPropertiesCache[generic];
        }
    }
}
