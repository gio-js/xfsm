using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using Xfsm.Core.Enums;
using Xfsm.Core.Interfaces;
using Xfsm.Core.Model;
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
            long id = connection.QueryFirst<long>(@"
                INSERT INTO dbo.XfsmElement (InsertedTimestamp, UpdatedTimestamp, PeekTimestamp, [State], PeekStatus, Error)
                VALUES (@now, @now, null, @state, @peek, null); 
                SELECT SCOPE_IDENTITY();",
                new XfsmDatabaseParameter("now", now),
                new XfsmDatabaseParameter("state", Convert.ToInt16(elementState)),
                new XfsmDatabaseParameter("peek", (byte)XfsmPeekStatus.Todo));

            connection.Execute(@"
                INSERT INTO dbo.XfsmBusinessElement (XfsmElementId, JsonData)
                VALUES (@id, @json);",
                new XfsmDatabaseParameter("id", id),
                new XfsmDatabaseParameter("json", JsonConvert.SerializeObject(businessElement)));

            connection.Commit();

            return id;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void EnsureInitialized()
        {
            using IXfsmDatabaseConnection connection = databaseProvider.GetConnection();
            int tableCount = connection.QueryFirst<int>("select count(*) from sys.tables where name in ('XfsmElement', 'XfsmBusinessElement');");

            if (tableCount == 2)
                return;

            throw new InvalidDataException("Database bag tables has not been properly initialized.");
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override XfsmElement<T> Peek(Enum state)
        {
            using IXfsmDatabaseConnection connection = databaseProvider.GetConnection();
            XfsmElementDto element = connection.QueryFirst<XfsmElementDto>(@"
with cte as (
select top 1 Id, InsertedTimestamp, UpdatedTimeStamp, PeekTimestamp, [State], PeekStatus, Error
from XfsmElement with(updlock,readpast)
where 
	[State] = @state
	and PeekStatus = 0 -- todo
order by Id
)
update cte set PeekStatus = 1
output inserted.*;
", new XfsmDatabaseParameter("state", state));

            XfsmBusinessElementDto businessElement = connection.QueryFirst<XfsmBusinessElementDto>(
                "select * from XfsmBusinessElement where Id = @id;", new XfsmDatabaseParameter("id", element.Id));


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