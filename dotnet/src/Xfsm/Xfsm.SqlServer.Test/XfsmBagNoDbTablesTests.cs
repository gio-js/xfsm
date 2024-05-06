using Xfsm.Core.Enums;
using Xfsm.Core.Interfaces;
using Xfsm.SqlServer.Test.Base;

namespace Xfsm.SqlServer.Test
{
    public class XfsmBagNoDbTablesTests : XfsmBaseTests
    {
        public class Sample
        {
            public long Id { get; set; }
            public string Code { get; set; }
        }

        [Test]
        [Order(1)]
        public void EnsureInitialized_NoTableHasBeenCreated_ThrowsException()
        {
            // ARRANGE
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(base.ConnectionString);
            using IXfsmDatabaseConnection connection = provider.GetConnection();
            XfsmPeekMode mode = XfsmPeekMode.Queue;
            XfsmBag<Sample> xfsm = new XfsmBag<Sample>(provider, mode);

            // ACT
            Assert.Throws<InvalidDataException>(() => xfsm.EnsureInitialized());
        }

        [Test]
        [Order(2)]
        public void RetrieveDDLScript_ReturnsValidScriptToInitilizeXfsm()
        {
            // ARRANGE
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(base.ConnectionString);
            using IXfsmDatabaseConnection connection = provider.GetConnection();
            XfsmPeekMode mode = XfsmPeekMode.Queue;
            XfsmBag<Sample> xfsm = new XfsmBag<Sample>(provider, mode);

            // ACT
            string script = xfsm.RetrieveDDLScript();
            connection.Execute(script);
        }
    }
}