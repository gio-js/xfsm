using Xfsm.Core.Interfaces;
using Xfsm.SqlServer.Test.Base;
using Xfsm.SqlServer.Test.Utils;

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
        public void Execute_SimpleGetDate_ReturnsServerDate()
        {
            // ARRANGE
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(base.ConnectionString);

            // ACT
            IXfsmDatabaseConnection connection = provider.OpenConnection();

            // ASSERT
            Assert.That(connection, Is.Not.Null);
        }
    }
}
