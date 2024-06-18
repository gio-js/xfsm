using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using Xfsm.Core;
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
    internal class XfsmBag<T> : Core.Abstract.XfsmBag<T>
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

            //TODO: create an abstraction for the serialization
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
        public override IXfsmElement<T> Peek(Enum state)
        {
            using IXfsmDatabaseConnection connection = databaseProvider.GetConnection();
            XfsmElementDto element = connection.QueryFirst<XfsmElementDto>(@"
with cte as (
select top 1 Id, InsertedTimestamp, UpdatedTimeStamp, PeekTimestamp, [State], PeekStatus, Error
from XfsmElement with(updlock,readpast)
where 
	[State] = @state
	and PeekStatus = @todo
order by Id
)
update cte set 
    PeekTimestamp = @peekts,
    PeekStatus = @progress
output inserted.*;
", new XfsmDatabaseParameter("state", state),
new XfsmDatabaseParameter("peekts", DateTimeProvider.Now()),
new XfsmDatabaseParameter("todo", XfsmPeekStatus.Todo),
new XfsmDatabaseParameter("progress", XfsmPeekStatus.Progress));

            if (element == null)
                return null;

            XfsmBusinessElementDto businessElement = connection.QueryFirst<XfsmBusinessElementDto>(
                "select * from XfsmBusinessElement where XfsmElementId = @id;", new XfsmDatabaseParameter("id", element.Id));
            connection.Commit();

            return new XfsmElement<T>(element, businessElement);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void Error(IXfsmElement<T> element, string errorMessage)
        {
            using IXfsmDatabaseConnection connection = databaseProvider.GetConnection();
            connection.Execute(@"
update XfsmElement set 
    UpdatedTimeStamp = @updts,
    PeekStatus = @error,
    Error = @errorMessage
where Id=@id;
", new XfsmDatabaseParameter("id", element.GetId()),
new XfsmDatabaseParameter("updts", DateTimeProvider.Now()),
new XfsmDatabaseParameter("error", XfsmPeekStatus.Error),
new XfsmDatabaseParameter("errorMessage", errorMessage));
            connection.Commit();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void Done(IXfsmElement<T> element)
        {
            using IXfsmDatabaseConnection connection = databaseProvider.GetConnection();
            connection.Execute(@"
update XfsmElement set 
    UpdatedTimeStamp = @updts,
    PeekStatus = @done
where Id=@id;
", new XfsmDatabaseParameter("id", element.GetId()),
new XfsmDatabaseParameter("updts", DateTimeProvider.Now()),
new XfsmDatabaseParameter("done", XfsmPeekStatus.Done));
            connection.Commit();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void Clear()
        {
            using IXfsmDatabaseConnection connection = databaseProvider.GetConnection();

            connection.Execute(@"
                TRUNCATE TABLE dbo.XfsmBusinessElement;
                DELETE FROM dbo.XfsmElement;");
            connection.Commit();
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