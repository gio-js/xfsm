using Moq;
using Xfsm.Core.Enums;
using Xfsm.Core.Interfaces;
using Xfsm.SqlServer.Test.Base;
using Xfsm.SqlServer.Test.Utils;

namespace Xfsm.SqlServer.Test
{
    public class XfsmBagTests : XfsmBaseTests
    {
        public class Sample
        {
            // empty
        }

        [Test]
        public void CanInstantiateObject()
        {
            // ARRANGE
            string connectionString = "";
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(connectionString);
            IXfsmState<Sample> state = new Mock<IXfsmState<Sample>>().Object;
            XfsmPeekMode mode = XfsmPeekMode.Queue;

            // ACT
            XfsmBag<Sample> xfsm = new XfsmBag<Sample>(provider, mode);

            // ASSERT
            Assert.That(provider, Is.Not.Null);
        }

        [Test]
        public void Ctor_EveryParamMandatory_ThrowsArgumentNullException()
        {
            // ARRANGE
            string connectionString = "";
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(connectionString);
            IXfsmState<Sample> state = new Mock<IXfsmState<Sample>>().Object;
            XfsmPeekMode mode = XfsmPeekMode.Queue;

            // ACT
            Assert.Throws<ArgumentNullException>(() => new XfsmBag<Sample>(null, mode));
        }

        [Test]
        public void RetrieveDDLScript_ReturnsDataModelScript()
        {
            // ARRANGE
            string connectionString = "";
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(connectionString);
            IXfsmState<Sample> state = new Mock<IXfsmState<Sample>>().Object;
            XfsmPeekMode mode = XfsmPeekMode.Queue;
            XfsmBag<Sample> xfsm = new XfsmBag<Sample>(provider, mode);
            string expectedScript = "Scripts.DataModel.sql".AsResourceString();

            // ACT
            string script = xfsm.RetrieveDDLScript();

            // ASSERT
            Assert.That(script, Is.EqualTo(expectedScript));
        }

        [Test]
        public void RetrieveDDLScript_ReturnsValidScriptToInitilizeXfsm()
        {
            // ARRANGE
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(base.ConnectionString);
            IXfsmDatabaseConnection connection = provider.GetConnection();
            IXfsmState<Sample> state = new Mock<IXfsmState<Sample>>().Object;
            XfsmPeekMode mode = XfsmPeekMode.Queue;
            XfsmBag<Sample> xfsm = new XfsmBag<Sample>(provider, mode);

            // ACT
            string script = xfsm.RetrieveDDLScript();
            connection.Execute(script);
        }
    }
}
