using Xfsm.Core.Interfaces;
using Xfsm.SqlServer.Test.Base;

namespace Xfsm.SqlServer.Test
{
    public class XfsmDatabaseProviderTests : XfsmBaseTests
    {

        [Test]
        public void CanInstantiateObject()
        {
            // ARRANGE
            string connectionString = "";

            // ACT
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(connectionString);

            // ASSERT
            Assert.That(provider, Is.Not.Null);
        }

        [Test]
        public void GetConnection_CreatesXfsmDatabaseConnection()
        {
            // ARRANGE
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(base.ConnectionString);

            // ACT
            IXfsmDatabaseConnection connection = provider.GetConnection();

            // ASSERT
            Assert.That(connection, Is.Not.Null);
        }

        [Test]
        public void GetConnection_MultipleCall_ReturnsDifferentInstances()
        {
            // ARRANGE
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(base.ConnectionString);

            // ACT
            IXfsmDatabaseConnection connection1 = provider.GetConnection();
            IXfsmDatabaseConnection connection2 = provider.GetConnection();

            // ASSERT
            Assert.That(connection1, Is.EqualTo(connection1));
            Assert.That(connection2, Is.EqualTo(connection2));
            Assert.That(connection1, Is.Not.EqualTo(connection2));
        }
    }
}
