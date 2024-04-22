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
        public void Execute_SimpleGetDate_ReturnsServerDate()
        {
            // ARRANGE
            IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(base.ConnectionString);

            // ACT
            DateTime serverDate = connection.Execute<DateTime>("select convert(datetime, '2023-09-11T09:08:11.000');");

            // ASSERT
            Assert.That(serverDate, Is.EqualTo("2023-09-11T09:08:11.000".ToDateTime()));
        }
    }
}
