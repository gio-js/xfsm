using Xfsm.Core.Interfaces;
using Xfsm.SqlServer.Test.Base;
using Xfsm.SqlServer.Test.Utils;

namespace Xfsm.SqlServer.Test
{
    public class XfsmDatabaseConnectionTests : XfsmBaseTests
    {
        [Test]
        public void CanInstantiateObject()
        {
            // ARRANGE
            string connectionString = "";

            // ACT
            IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(connectionString);

            // ASSERT
            Assert.That(connection, Is.Not.Null);
        }

        [Test]
        public void Query_SimpleGetDate_ReturnsServerDate()
        {
            // ARRANGE
            IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(base.ConnectionString);

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
            IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(base.ConnectionString);

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
            IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(base.ConnectionString);

            // ACT
            IList<string> result = connection.Query<string>($"select 'test';");

            // ASSERT
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First(), Is.EqualTo("test"));
        }

        public class SimpleDto
        {
            public int Column1 { get; set; }
            public string Column2 { get; set; }
        }

        [Test]
        public void Query_GetSimpleDto()
        {
            // ARRANGE
            IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(base.ConnectionString);

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
            IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(base.ConnectionString);

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
    }
}
