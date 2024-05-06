using Microsoft.Extensions.Configuration;

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
        }
    }
}
