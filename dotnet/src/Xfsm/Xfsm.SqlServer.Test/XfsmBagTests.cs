using Xfsm.Core.Enums;
using Xfsm.Core.Interfaces;
using Xfsm.Core.Model;
using Xfsm.SqlServer.Internal;
using Xfsm.SqlServer.Test.Base;
using Xfsm.SqlServer.Test.Utils;

namespace Xfsm.SqlServer.Test
{
    public class XfsmBagTests : XfsmBaseTests
    {
        public class Sample
        {
            public long Id { get; set; }
            public string Code { get; set; }
        }

        public enum SampleEnum
        {
            State1,
            State2,
        }

        [Test]
        public void CanInstantiateObject()
        {
            // ARRANGE
            string connectionString = "";
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(connectionString);
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
            XfsmPeekMode mode = XfsmPeekMode.Queue;
            XfsmBag<Sample> xfsm = new XfsmBag<Sample>(provider, mode);

            // ACT
            string script = xfsm.RetrieveDDLScript();
            connection.Execute(script);
        }

        [Test]
        public void AddElement_NullElement_ThrowsArgumentNullException()
        {
            // ARRANGE
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(base.ConnectionString);
            IXfsmDatabaseConnection connection = provider.GetConnection();
            XfsmPeekMode mode = XfsmPeekMode.Queue;
            XfsmBag<Sample> xfsm = new XfsmBag<Sample>(provider, mode);

            // ACT
            Assert.Throws<ArgumentNullException>(() => xfsm.AddElement(null, SampleEnum.State1));
        }

        [Test]
        public void AddElement_CreatesElement()
        {
            // ARRANGE
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(base.ConnectionString);
            IXfsmDatabaseConnection connection = provider.GetConnection();
            XfsmPeekMode mode = XfsmPeekMode.Queue;
            XfsmBag<Sample> xfsm = new XfsmBag<Sample>(provider, mode);
            DateTimeOffset offset = "2023-09-11T09:08:11.000+01:00".ToDateTimeOffset();
            DateTimeProvider.Set(offset);

            // ACT
            long id = xfsm.AddElement(new Sample { Id = 1, Code = "Code1" }, SampleEnum.State1);

            // ASSERT
            XfsmElementDto dto = connection.QueryFirst<XfsmElementDto>($"select * from dbo.XfsmElement where Id={id};");
            XfsmBusinessElementDto json = connection.QueryFirst<XfsmBusinessElementDto>($"select * from dbo.XfsmBusinessElement where XfsmElementId={id};");

            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Id, Is.EqualTo(id));
            Assert.That(dto.InsertedTimestamp, Is.EqualTo(offset));
            Assert.That(dto.UpdatedTimestamp, Is.EqualTo(offset));
            Assert.That(dto.PeekTimestamp, Is.Null);
            Assert.That(dto.State, Is.EqualTo(SampleEnum.State1));
            Assert.That(dto.PeekStatus, Is.EqualTo(XfsmPeekStatus.Todo));
            Assert.That(dto.Error, Is.Not.Null);

            Assert.That(json, Is.Not.Null);
            Assert.That(json.XfsmElementId, Is.EqualTo(id));
            Assert.That(json.JsonData, Is.EqualTo("{ Id: 1, Code: 'Code1' }"));
        }
    }
}