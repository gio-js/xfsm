using Moq;
using Xfsm.Core.Enums;
using Xfsm.Core.Interfaces;
using Xfsm.SqlServer.Test.Base;
using Xfsm.SqlServer.Test.Utils;

namespace Xfsm.SqlServer.Test
{
    public class XfsmTests : XfsmBaseTests
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
            IXfsmState state = new Mock<IXfsmState>().Object;
            XfsmFetchMode mode = XfsmFetchMode.Queue;

            // ACT
            Xfsm<Sample> xfsm = new Xfsm<Sample>(state, state, provider, mode);

            // ASSERT
            Assert.That(provider, Is.Not.Null);
        }

        [Test]
        public void Ctor_EveryParamMandatory_ThrowsArgumentNullException()
        {
            // ARRANGE
            string connectionString = "";
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(connectionString);
            IXfsmState state = new Mock<IXfsmState>().Object;
            XfsmFetchMode mode = XfsmFetchMode.Queue;

            // ACT
            Assert.Throws<ArgumentNullException>(() => new Xfsm<Sample>(null, state, provider, mode));
            Assert.Throws<ArgumentNullException>(() => new Xfsm<Sample>(state, null, provider, mode));
            Assert.Throws<ArgumentNullException>(() => new Xfsm<Sample>(state, state, null, mode));
        }

        [Test]
        public void RetrieveDDLScript_ReturnsDataModelScript()
        {
            // ARRANGE
            string connectionString = "";
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(connectionString);
            IXfsmState state = new Mock<IXfsmState>().Object;
            XfsmFetchMode mode = XfsmFetchMode.Queue;
            Xfsm<Sample> xfsm = new Xfsm<Sample>(state, state, provider, mode);
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
            IXfsmState state = new Mock<IXfsmState>().Object;
            XfsmFetchMode mode = XfsmFetchMode.Queue;
            Xfsm<Sample> xfsm = new Xfsm<Sample>(state, state, provider, mode);

            // ACT
            string script = xfsm.RetrieveDDLScript();
            connection.Execute(script);
        }
    }
}
