using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Xfsm.Core.Interfaces;

namespace Xfsm.SqlServer.Test
{
    public class XfsmDatabaseConnectionTests
    {
        [Test]
        public void CanInstantiateObject()
        {
            // ARRANGE
            string connectionString = "";

            // ACT
            IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(connectionString);

            // ASSERT
        }

        [Test]
        public void Execute_SimpleGetDate_ReturnsServerDate()
        {
            // ARRANGE
            string connectionString = "Data Source=127.0.0.1,1433;User=sa;Password=Pass@word;TrustServerCertificate=true";
            IXfsmDatabaseConnection connection = new XfsmDatabaseConnection(connectionString);

            // ACT
            DateTime serverDate = connection.Execute<DateTime>("select getdate();");

            // ASSERT
            Assert.That(serverDate, Is.Not.Empty);
        }
    }
}
