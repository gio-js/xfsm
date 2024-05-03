using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using Xfsm.Core.Enums;
using Xfsm.Core.Interfaces;
using Xfsm.SqlServer.Extensions;
using Xfsm.SqlServer.Internal;

namespace Xfsm.SqlServer
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class XfsmBag<T> : Core.Abstract.XfsmBag<T>
    {
        public XfsmBag(Core.Abstract.XfsmDatabaseProvider databaseProvider, XfsmPeekMode fetchMode) : base(databaseProvider, fetchMode) { }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override long AddElement(T businessElement, Enum elementState)
        {
            if (businessElement == null)
                throw new ArgumentNullException(nameof(businessElement));

            string now = DateTimeProvider.Now().ToSql();

            using IXfsmDatabaseConnection connection = databaseProvider.GetConnection();
            long id = connection.QueryFirst<long>(string.Format(@"
                INSERT INTO dbo.XfsmElement (InsertedTimestamp, UpdatedTimestamp, PeekTimestamp, [State], PeekStatus, Error)
                VALUES ('{0}', '{1}', null, {2}, {3}, null); 
                SELECT SCOPE_IDENTITY();", now, now, Convert.ToInt16(elementState), (byte)XfsmPeekStatus.Todo));

            connection.Execute(string.Format(@"
                INSERT INTO dbo.XfsmBusinessElement (XfsmElementId, JsonData)
                VALUES ('{0}', '{1}');", id, JsonConvert.SerializeObject(businessElement))); //todo: transform to  sql parameters

            connection.Commit();

            return id;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void EnsureInitialized()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override IXfsmElement<T> Peek(Enum state)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override string RetrieveDDLScript()
        {
            Assembly libAssembly = Assembly.GetAssembly(typeof(XfsmDatabaseProvider));
            using Stream stream = libAssembly.GetManifestResourceStream($"Xfsm.SqlServer.Scripts.DataModel.sql");
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}