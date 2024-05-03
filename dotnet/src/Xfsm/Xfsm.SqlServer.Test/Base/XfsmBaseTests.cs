using Microsoft.Extensions.Configuration;
using Xfsm.Core.Interfaces;
using Xfsm.SqlServer.Test.Utils;

namespace Xfsm.SqlServer.Test.Base
{
    public abstract class XfsmBaseTests
    {
        protected string ConnectionString { private set; get; } = "";

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            // tests configuration
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            this.ConnectionString = configuration.GetConnectionString("TestConnectionString");

            // assert on configuration
            Assert.That(this.ConnectionString, Is.Not.Null, "Connection string is null.");
            Assert.That(this.ConnectionString, Is.Not.Empty, "Connection string is empty.");

            // initialize bag tables
            string script = "Scripts.DataModel.sql".AsResourceString();
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(ConnectionString);
            using IXfsmDatabaseConnection connection = provider.GetConnection();
            connection.Execute(script);
            connection.Commit();
        }
    }
}
