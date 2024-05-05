﻿using Xfsm.Core.Interfaces;
using Xfsm.SqlServer.Test.Base;
using Xfsm.SqlServer.Test.Utils;

namespace Xfsm.SqlServer.Test
{
    public class SimpleDto
    {
        public int Column1 { get; set; }
        public string Column2 { get; set; }
    }

    public class DDLTableDto
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class XfsmDatabaseConnectionTests : XfsmBaseTests
    {
        [Test]
        public void CanInstantiateObject()
        {
            // ARRANGE
            string connectionString = "the_connection_string";

            // ACT
            using IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(connectionString);

            // ASSERT
            Assert.That(connection, Is.Not.Null);
        }

        [Test]
        public void CanInstantiateObject_GivenEmptyOrNull_ThrowsNullArgEx()
        {
            // ACT
            Assert.Throws<ArgumentNullException>(() => new XfsmDatabaseConnection(""));
            Assert.Throws<ArgumentNullException>(() => new XfsmDatabaseConnection(null));
        }

        [Test]
        public void Query_SimpleGetDate_ReturnsServerDate()
        {
            // ARRANGE
            using IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(base.ConnectionString);

            // ACT
            IList<DateTime> result = connection.Query<DateTime>("select convert(datetime, '2023-09-11T09:08:11.000');");

            // ASSERT
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First(), Is.EqualTo("2023-09-11T09:08:11.000".ToDateTime()));
        }

        [Test]
        public void Query_SimpleGetInt_ReturnsInt([Values(-12312, 0, 412412)] int value)
        {
            // ARRANGE
            using IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(base.ConnectionString);

            // ACT
            IList<int> result = connection.Query<int>($"select convert(int, {value});");

            // ASSERT
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First(), Is.EqualTo(value));
        }

        [Test]
        public void Query_SimpleGetString_ReturnsString()
        {
            // ARRANGE
            using IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(base.ConnectionString);

            // ACT
            IList<string> result = connection.Query<string>($"select 'test';");

            // ASSERT
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First(), Is.EqualTo("test"));
        }

        [Test]
        public void Query_GetSimpleDto()
        {
            // ARRANGE
            using IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(base.ConnectionString);

            // ACT
            IList<SimpleDto> result = connection.Query<SimpleDto>($"select 123 as Column1, 'test' as Column2;");

            // ASSERT
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Column1, Is.EqualTo(123));
            Assert.That(result.First().Column2, Is.EqualTo("test"));
        }

        [Test]
        public void Query_GetSimpleDtoArray()
        {
            // ARRANGE
            using IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(base.ConnectionString);

            // ACT
            IList<SimpleDto> result = connection.Query<SimpleDto>($"select 123 as Column1, 'test1' as Column2 union select 456 as Column1, 'test2' as Column2;");

            // ASSERT
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Column1, Is.EqualTo(123));
            Assert.That(result[0].Column2, Is.EqualTo("test1"));
            Assert.That(result[1].Column1, Is.EqualTo(456));
            Assert.That(result[1].Column2, Is.EqualTo("test2"));
        }

        [Test]
        public void QueryFirst_GetSimpleDtoArray()
        {
            // ARRANGE
            using IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(base.ConnectionString);

            // ACT
            SimpleDto dto = connection.QueryFirst<SimpleDto>($"select 123 as Column1, 'test1' as Column2 union select 456 as Column1, 'test2' as Column2;");

            // ASSERT
            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Column1, Is.EqualTo(123));
            Assert.That(dto.Column2, Is.EqualTo("test1"));
        }

        [Test]
        public void QueryFirst_EmptyArray_GetSimpleDtoArray()
        {
            // ARRANGE
            using IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(base.ConnectionString);

            // ACT
            SimpleDto dto = connection.QueryFirst<SimpleDto>($"select top 0 null as Column2;");

            // ASSERT
            Assert.That(dto, Is.Null);
        }

        [Test]
        public void Query_WithParameters_GetSimpleDtoArray()
        {
            // ARRANGE
            using IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(base.ConnectionString);

            // ACT
            IList<SimpleDto> list = connection.Query<SimpleDto>($@"
                select * from (
                    select 1 Column1, 'Description1' Column2
                    union select 2 Column1, 'Description2' Column2
                    union select 3 Column1, 'Description3' Column2
                ) a
                where Column1 = @id", new XfsmDatabaseParameter("id", 2));

            // ASSERT
            Assert.That(list, Is.Not.Null);
            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(list.First().Column1, Is.EqualTo(2));
            Assert.That(list.First().Column2, Is.EqualTo("Description2"));
        }

        [Test]
        public void Query_WithParameters_GetTwoSimpleDtoArray()
        {
            // ARRANGE
            using IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(base.ConnectionString);

            // ACT
            IList<SimpleDto> list = connection.Query<SimpleDto>($@"
                select * from (
                    select 1 Column1, 'Description1' Column2
                    union select 2 Column1, 'Description2' Column2
                    union select 3 Column1, 'Description3' Column2
                ) a
                where Column1 in (@id1, @id2) ", new XfsmDatabaseParameter("id1", 3), new XfsmDatabaseParameter("id2", 2));

            // ASSERT
            Assert.That(list, Is.Not.Null);
            Assert.That(list.Count, Is.EqualTo(2));

            var dto = list.First(x => x.Column1 == 3);
            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Column1, Is.EqualTo(3));
            Assert.That(dto.Column2, Is.EqualTo("Description3"));

            dto = list.First(x => x.Column1 == 2);
            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Column1, Is.EqualTo(2));
            Assert.That(dto.Column2, Is.EqualTo("Description2"));
        }

        [Test]
        public void QueryFirst_WithParameters_GetSimpleDto()
        {
            // ARRANGE
            using IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(base.ConnectionString);

            // ACT
            SimpleDto dto = connection.QueryFirst<SimpleDto>($@"
                select * from (
                    select 1 Column1, 'Description1' Column2
                    union select 2 Column1, 'Description2' Column2
                    union select 3 Column1, 'Description3' Column2
                ) a
                where Column1 = @id", new XfsmDatabaseParameter("id", 2));

            // ASSERT
            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Column1, Is.EqualTo(2));
            Assert.That(dto.Column2, Is.EqualTo("Description2"));
        }

        [Test]
        public void Execute_RunDDL()
        {
            // ARRANGE
            using IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(base.ConnectionString);
            string uuid = Guid.NewGuid().ToString();
            string table = $"[tbl_{uuid}]";

            // ACT
            connection.Execute($"create table {table} (Id int, Value nvarchar(256));");
            IList<DDLTableDto> result = connection.Query<DDLTableDto>($"select * from {table};");

            // ASSERT
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Execute_RunDML()
        {
            // ARRANGE
            using IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(base.ConnectionString);
            string uuid = Guid.NewGuid().ToString();
            string table = $"[tbl_{uuid}]";
            IList<DDLTableDto> result = null;

            // ACT
            connection.Execute($"create table {table} (Id int, Value nvarchar(256));");
            connection.Execute($"insert into {table} (Id, Value) values (10, 'Value-10');");

            // ASSERT
            result = connection.Query<DDLTableDto>($"select * from {table};");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo(10));
            Assert.That(result.First().Value, Is.EqualTo("Value-10"));

            // ACT
            connection.Execute($"update {table} set Value = 'Value-20' where Id = 10;");

            // ASSERT
            result = connection.Query<DDLTableDto>($"select * from {table};");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo(10));
            Assert.That(result.First().Value, Is.EqualTo("Value-20"));

            // ACT
            connection.Execute($"insert into {table} (Id, Value) values (30, 'Value-30');");

            // ASSERT
            result = connection.Query<DDLTableDto>($"select * from {table};");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.First(x => x.Id == 30).Id, Is.EqualTo(30));
            Assert.That(result.First(x => x.Id == 30).Value, Is.EqualTo("Value-30"));

            // ACT
            connection.Execute($"delete from {table} where Id = 10;");

            // ASSERT
            result = connection.Query<DDLTableDto>($"select * from {table};");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo(30));
            Assert.That(result.First().Value, Is.EqualTo("Value-30"));
        }

        [Test]
        public void Execute_Test_NoCommit_RollbackOnDispose()
        {
            // ARRANGE
            using IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(base.ConnectionString);
            string uuid = Guid.NewGuid().ToString();
            string table = $"tbl_{uuid}";

            // ACT
            connection.Execute($"create table [{table}] (Id int, Value nvarchar(256));");
            IList<int> tableCount = connection.Query<int>($"select count(*) from sys.tables where name = '{table}';");

            // ASSERT
            Assert.That(tableCount, Is.Not.Null);
            Assert.That(tableCount, Is.Not.Empty);
            Assert.That(tableCount.First(), Is.EqualTo(1));

            // ACT
            connection.Dispose(); // connection dispose
            tableCount = connection.Query<int>($"select count(*) from sys.tables where name = '{table}';");

            // ASSERT
            Assert.That(tableCount, Is.Not.Null);
            Assert.That(tableCount, Is.Not.Empty);
            Assert.That(tableCount.First(), Is.EqualTo(0));
        }

        [Test]
        public void Execute_Test_Commit_NoRollbackOnDispose()
        {
            // ARRANGE
            using IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(base.ConnectionString);
            string uuid = Guid.NewGuid().ToString();
            string table = $"tbl_{uuid}";

            // ACT
            connection.Execute($"create table [{table}] (Id int, Value nvarchar(256));");
            IList<int> tableCount = connection.Query<int>($"select count(*) from sys.tables where name = '{table}';");

            // ASSERT
            Assert.That(tableCount, Is.Not.Null);
            Assert.That(tableCount, Is.Not.Empty);
            Assert.That(tableCount.First(), Is.EqualTo(1));

            // ACT
            connection.Commit(); // execute commit on current connection/transaction
            tableCount = connection.Query<int>($"select count(*) from sys.tables where name = '{table}';");

            // ASSERT
            Assert.That(tableCount, Is.Not.Null);
            Assert.That(tableCount, Is.Not.Empty);
            Assert.That(tableCount.First(), Is.EqualTo(1));

            // ACT
            connection.Dispose(); // dispose connection and related transaction
            tableCount = connection.Query<int>($"select count(*) from sys.tables where name = '{table}';");

            // ASSERT
            Assert.That(tableCount, Is.Not.Null);
            Assert.That(tableCount, Is.Not.Empty);
            Assert.That(tableCount.First(), Is.EqualTo(1));
        }

        [Test]
        public void Execute_Test_NoTransaction_Commit_ThrowsException()
        {
            // ARRANGE
            using IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(base.ConnectionString);

            // ACT
            Exception exception = Assert.Throws<Exception>(() => connection.Commit());

            // ASSERT
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("No transaction available."));
        }

        [Test]
        public void Execute_Test_WithParameters()
        {
            // ARRANGE
            using IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(base.ConnectionString);
            string uuid = Guid.NewGuid().ToString();
            string table = $"tbl_{uuid}";

            // ACT
            connection.Execute($"create table [{table}] (Id int, Value nvarchar(256));");
            connection.Execute($"insert into [{table}] (Id, Value) values (@id, @value);",
                new XfsmDatabaseParameter("id", 1),
                new XfsmDatabaseParameter("value", "Value2"));

            int tableCount = connection.QueryFirst<int>($"select count(*) from [{table}] where Id = 1;");

            // ASSERT
            Assert.That(tableCount, Is.EqualTo(1));
        }
    }
}
