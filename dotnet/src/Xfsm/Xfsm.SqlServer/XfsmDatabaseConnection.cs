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

        private string[] exPrimitiveTypes = new string[] { "DateTime", "String" };
        private Dictionary<Type, PropertyInfo[]> typesPropertiesCache = new Dictionary<Type, PropertyInfo[]>();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IList<T> Query<T>(string sqlQuery)
        {
            if (this.connection == null)
            {
                this.connection = new SqlConnection(connectionString);
                this.connection.Open();
            }

            Type generic = typeof(T);

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = sqlQuery;
                using (SqlDataReader reader = command.ExecuteReader())
                {
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
            }
        }

        private PropertyInfo[] GetElementProperties(Type generic)
        {
            if (!typesPropertiesCache.ContainsKey(generic))
                typesPropertiesCache[generic] = generic.GetProperties();

            return typesPropertiesCache[generic];
        }
    }
}
