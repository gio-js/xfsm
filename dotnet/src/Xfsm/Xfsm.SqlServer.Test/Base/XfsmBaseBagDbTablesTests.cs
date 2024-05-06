using Xfsm.Core.Interfaces;
using Xfsm.SqlServer.Test.Utils;

namespace Xfsm.SqlServer.Test.Base
{
    public abstract class XfsmBaseBagDbTablesTests : XfsmBaseTests
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            // initialize bag tables
            string script = "Scripts.DataModel.sql".AsResourceString();
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(ConnectionString);
            using IXfsmDatabaseConnection connection = provider.GetConnection();
            connection.Execute(script);
            connection.Commit();
        }
    }
}
