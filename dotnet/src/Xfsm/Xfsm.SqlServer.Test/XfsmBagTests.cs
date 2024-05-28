using Moq;
using System.Collections.Concurrent;
using Xfsm.Core.Enums;
using Xfsm.Core.Interfaces;
using Xfsm.Core.Model;
using Xfsm.SqlServer.Internal;
using Xfsm.SqlServer.Test.Base;
using Xfsm.SqlServer.Test.Utils;

namespace Xfsm.SqlServer.Test
{
    [NonParallelizable]
    public class XfsmBagTests : XfsmBaseBagDbTablesTests
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

        private XfsmDatabaseProvider _provider;


        [SetUp]
        public void Setup()
        {
            if (_provider == null)
                _provider = new XfsmDatabaseProvider(base.ConnectionString);

            XfsmBag<Sample> xfsm = new XfsmBag<Sample>(_provider, default);

            // clear bag
            xfsm.Clear();
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
        public void AddElement_NullElement_ThrowsArgumentNullException()
        {
            // ARRANGE
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(base.ConnectionString);
            using IXfsmDatabaseConnection connection = provider.GetConnection();
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
            using IXfsmDatabaseConnection connection = provider.GetConnection();
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
            Assert.That(dto.State, Is.EqualTo((int)SampleEnum.State1));
            Assert.That(dto.PeekStatus, Is.EqualTo(XfsmPeekStatus.Todo));
            Assert.That(dto.Error, Is.Null);

            Assert.That(json, Is.Not.Null);
            Assert.That(json.XfsmElementId, Is.EqualTo(id));
            Assert.That(json.JsonData, Is.EqualTo("{\"Id\":1,\"Code\":\"Code1\"}"));
        }

        [Test]
        public void Clear_GivenTwoAdds_AfterClear_ReturnsEmptyTable()
        {
            // ARRANGE
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(base.ConnectionString);
            using IXfsmDatabaseConnection connection = provider.GetConnection();
            XfsmBag<Sample> xfsm = new XfsmBag<Sample>(provider, XfsmPeekMode.Queue);
            xfsm.AddElement(new Sample { Id = 1 }, SampleEnum.State1);
            xfsm.AddElement(new Sample { Id = 1 }, SampleEnum.State1);
            int count = connection.QueryFirst<int>($"select count(*) from dbo.XfsmElement;");
            Assert.That(count, Is.EqualTo(2));

            // ACT
            xfsm.Clear();

            // ASSERT
            count = connection.QueryFirst<int>($"select count(*) from dbo.XfsmElement;");
            Assert.That(count, Is.EqualTo(0));
        }

        [Test]
        public void EnsureInitialized_ExistingTable_NoException()
        {
            // ARRANGE
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(base.ConnectionString);
            XfsmPeekMode mode = XfsmPeekMode.Queue;
            XfsmBag<Sample> xfsm = new XfsmBag<Sample>(provider, mode);

            // ACT
            // not needed, generated by base class
            //string script = xfsm.RetrieveDDLScript();
            //connection.Execute(script);

            xfsm.EnsureInitialized();
        }

        [Test]
        public void GetPeekMode_ReturnsModeGivenByCtor([Values(XfsmPeekMode.Stack, XfsmPeekMode.Queue)] XfsmPeekMode mode)
        {
            // ARRANGE
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(base.ConnectionString);
            XfsmBag<Sample> xfsm = new XfsmBag<Sample>(provider, mode);

            // ACT
            XfsmPeekMode result = xfsm.GetPeekMode();

            // ASSERT
            Assert.That(result, Is.EqualTo(mode));
        }

        [Test]
        public void Peek_QueueMode_Given3ElementsInState1_PeekOnState1_ReturnsFirstInserted()
        {
            // ARRANGE
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(base.ConnectionString);
            XfsmPeekMode mode = XfsmPeekMode.Queue;
            XfsmBag<Sample> xfsm = new XfsmBag<Sample>(provider, mode);
            DateTimeOffset inserted = "2023-08-14T14:08:16.000+01:00".ToDateTimeOffset();
            DateTimeOffset peeked = "2023-08-14T14:08:16.000+01:00".ToDateTimeOffset();
            DateTimeProvider.Set(inserted);

            long id1 = xfsm.AddElement(new Sample { Id = 100, Code = "100" }, SampleEnum.State1);
            long id2 = xfsm.AddElement(new Sample { Id = 200, Code = "200" }, SampleEnum.State1);
            long id3 = xfsm.AddElement(new Sample { Id = 300, Code = "300" }, SampleEnum.State1);

            // ACT
            DateTimeProvider.Set(peeked);
            IXfsmElement<Sample> result = xfsm.Peek(SampleEnum.State1);

            // ASSERT
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetId(), Is.EqualTo(id1));
            Assert.That(result.GetBusinessElement(), Is.Not.Null);
            Assert.That(result.GetInsertedTimestamp(), Is.EqualTo(inserted));
            Assert.That(result.GetPeekedTimestamp(), Is.EqualTo(peeked));
            Assert.That(result.GetLastUpdateTimestamp(), Is.EqualTo(peeked));
            Assert.That(result.GetState(), Is.EqualTo((int)SampleEnum.State1));
            Assert.That(result.GetPeekStatus(), Is.EqualTo(XfsmPeekStatus.Progress));
            Assert.That(result.GetError(), Is.Null);

            Sample businessElement = result.GetBusinessElement();
            Assert.That(businessElement.Id, Is.EqualTo(100));
            Assert.That(businessElement.Code, Is.EqualTo("100"));
        }

        [Test]
        public void Peek_QueueMode_Given3Elements_OnlyMiddleInState1_PeekOnState1_ReturnsMiddleElement()
        {
            // ARRANGE
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(base.ConnectionString);
            XfsmPeekMode mode = XfsmPeekMode.Queue;
            XfsmBag<Sample> xfsm = new XfsmBag<Sample>(provider, mode);
            DateTimeOffset inserted = "2023-08-14T14:08:16.000+01:00".ToDateTimeOffset();
            DateTimeOffset peeked = "2023-08-14T14:08:16.000+01:00".ToDateTimeOffset();
            DateTimeProvider.Set(inserted);

            long id1 = xfsm.AddElement(new Sample { Id = 100, Code = "100" }, SampleEnum.State2);
            long id2 = xfsm.AddElement(new Sample { Id = 200, Code = "200" }, SampleEnum.State1);
            long id3 = xfsm.AddElement(new Sample { Id = 300, Code = "300" }, SampleEnum.State2);

            // ACT
            DateTimeProvider.Set(peeked);
            IXfsmElement<Sample> result = xfsm.Peek(SampleEnum.State1);

            // ASSERT
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetId(), Is.EqualTo(id2));
            Assert.That(result.GetBusinessElement(), Is.Not.Null);
            Assert.That(result.GetInsertedTimestamp(), Is.EqualTo(inserted));
            Assert.That(result.GetPeekedTimestamp(), Is.EqualTo(peeked));
            Assert.That(result.GetLastUpdateTimestamp(), Is.EqualTo(peeked));
            Assert.That(result.GetState(), Is.EqualTo((int)SampleEnum.State1));
            Assert.That(result.GetPeekStatus(), Is.EqualTo(XfsmPeekStatus.Progress));
            Assert.That(result.GetError(), Is.Null);

            Sample businessElement = result.GetBusinessElement();
            Assert.That(businessElement.Id, Is.EqualTo(200));
            Assert.That(businessElement.Code, Is.EqualTo("200"));
        }

        [Test]
        public void Peek_QueueMode_Given3Elements_InState1_PeekThreeTimesOnState1_ReturnsThirdElement()
        {
            // ARRANGE
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(base.ConnectionString);
            XfsmPeekMode mode = XfsmPeekMode.Queue;
            XfsmBag<Sample> xfsm = new XfsmBag<Sample>(provider, mode);
            DateTimeOffset inserted = "2023-08-14T14:08:16.000+01:00".ToDateTimeOffset();
            DateTimeOffset peeked = "2023-08-14T14:08:16.000+01:00".ToDateTimeOffset();
            DateTimeProvider.Set(inserted);

            long id1 = xfsm.AddElement(new Sample { Id = 700, Code = "700" }, SampleEnum.State1);
            long id2 = xfsm.AddElement(new Sample { Id = 800, Code = "800" }, SampleEnum.State1);
            long id3 = xfsm.AddElement(new Sample { Id = 900, Code = "900" }, SampleEnum.State1);

            // ACT
            DateTimeProvider.Set(peeked);
            IXfsmElement<Sample> result = null;
            result = xfsm.Peek(SampleEnum.State1); // first
            result = xfsm.Peek(SampleEnum.State1); // second
            result = xfsm.Peek(SampleEnum.State1); // third

            // ASSERT
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetId(), Is.EqualTo(id3));
            Assert.That(result.GetBusinessElement(), Is.Not.Null);
            Assert.That(result.GetInsertedTimestamp(), Is.EqualTo(inserted));
            Assert.That(result.GetPeekedTimestamp(), Is.EqualTo(peeked));
            Assert.That(result.GetLastUpdateTimestamp(), Is.EqualTo(peeked));
            Assert.That(result.GetState(), Is.EqualTo((int)SampleEnum.State1));
            Assert.That(result.GetPeekStatus(), Is.EqualTo(XfsmPeekStatus.Progress));
            Assert.That(result.GetError(), Is.Null);

            Sample businessElement = result.GetBusinessElement();
            Assert.That(businessElement.Id, Is.EqualTo(900));
            Assert.That(businessElement.Code, Is.EqualTo("900"));
        }

        [Test]
        public void Peek_QueueMode_Given3Elements_InState1_PeekState2_ReturnsNull()
        {
            // ARRANGE
            XfsmPeekMode mode = XfsmPeekMode.Queue;
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(base.ConnectionString);
            XfsmBag<Sample> xfsm = new XfsmBag<Sample>(provider, mode);

            xfsm.AddElement(new Sample { Id = 100, Code = "100" }, SampleEnum.State1);
            xfsm.AddElement(new Sample { Id = 200, Code = "200" }, SampleEnum.State1);
            xfsm.AddElement(new Sample { Id = 300, Code = "300" }, SampleEnum.State1);

            // ACT
            IXfsmElement<Sample> result = xfsm.Peek(SampleEnum.State2);

            // ASSERT
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Peek_QueueMode_Given3Elements_InState1_PeekFourTimesOnState1_LastPeekReturnsNull()
        {
            // ARRANGE
            XfsmPeekMode mode = XfsmPeekMode.Queue;
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(base.ConnectionString);
            XfsmBag<Sample> xfsm = new XfsmBag<Sample>(provider, mode);

            xfsm.AddElement(new Sample { Id = 700, Code = "700" }, SampleEnum.State1);
            xfsm.AddElement(new Sample { Id = 800, Code = "800" }, SampleEnum.State1);
            xfsm.AddElement(new Sample { Id = 900, Code = "900" }, SampleEnum.State1);

            // ACT
            IXfsmElement<Sample> result = null;
            result = xfsm.Peek(SampleEnum.State1); // first
            result = xfsm.Peek(SampleEnum.State1); // second
            result = xfsm.Peek(SampleEnum.State1); // third
            result = xfsm.Peek(SampleEnum.State1); // fourth

            // ASSERT
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Peek_QueueMode_Given3Elements_InState1_PeekThreeTimes_PeeksElementsInTheRightOrder()
        {
            // ARRANGE
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(base.ConnectionString);
            XfsmPeekMode mode = XfsmPeekMode.Queue;
            XfsmBag<Sample> xfsm = new XfsmBag<Sample>(provider, mode);
            DateTimeOffset inserted = "2023-08-14T14:08:16.000+01:00".ToDateTimeOffset();
            DateTimeOffset peeked = "2023-08-14T14:08:16.000+01:00".ToDateTimeOffset();
            DateTimeProvider.Set(inserted);

            long id1 = xfsm.AddElement(new Sample { Id = 700, Code = "700" }, SampleEnum.State1);
            long id2 = xfsm.AddElement(new Sample { Id = 800, Code = "800" }, SampleEnum.State1);
            long id3 = xfsm.AddElement(new Sample { Id = 900, Code = "900" }, SampleEnum.State1);

            // ACT
            DateTimeProvider.Set(peeked);
            IList<IXfsmElement<Sample>> result = new List<IXfsmElement<Sample>>();

            result.Add(xfsm.Peek(SampleEnum.State1));
            result.Add(xfsm.Peek(SampleEnum.State1));
            result.Add(xfsm.Peek(SampleEnum.State1));

            // ASSERT
            Assert.That(new List<long>() { id1, id2, id3 }, Is.EqualTo(result.Select(x => x.GetId())).AsCollection);
        }

        [Test]
        public void Peek_QueueMode_GivenLotsOfElements_PeekInParallel_UniqueElementsInDifferentCollections(
            [Values(100, 1000, 5000)] int n)
        {
            // ARRANGE
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(base.ConnectionString);
            XfsmPeekMode mode = XfsmPeekMode.Queue;
            XfsmBag<Sample> xfsm = new XfsmBag<Sample>(provider, mode);
            int nActors = 5;
            Random rnd = new Random();

            for (int i = 0; i < n; i++)
            {
                xfsm.AddElement(new Sample { Id = 700, Code = "700" }, SampleEnum.State1);
            }

            // ACT
            ConcurrentDictionary<int, List<long>> elements = new ConcurrentDictionary<int, List<long>>();

            Parallel.For(0, nActors, (index) =>
            {
                XfsmDatabaseProvider innerProvider = new XfsmDatabaseProvider(base.ConnectionString);
                XfsmBag<Sample> innerXfsm = new XfsmBag<Sample>(innerProvider, mode);
                IXfsmElement<Sample> element = null;

                if (!elements.ContainsKey(index))
                    elements.TryAdd(index, new List<long>());

                do
                {
                    // dequeue 
                    element = xfsm.Peek(SampleEnum.State1);

                    // add id
                    if (element != null)
                        elements[index].Add(element.GetId());

                    Task.Delay(rnd.Next(70) + 30).Wait();

                } while (element != null);
            });

            // ASSERT
            Assert.That(nActors, Is.EqualTo(elements.Keys.Count));
            Assert.That(n, Is.EqualTo(elements.SelectMany(x => x.Value).Select(y => y).Distinct().Count())); // dequeued all elements

            var groupedById = elements.SelectMany(x => x.Value).GroupBy(y => y).Select(z => new { z.Key, Count = z.Count() });
            Assert.That(groupedById.Count(), Is.EqualTo(n)); // again, dequeued all elements
            Assert.That(groupedById.Where(x => x.Count > 1).Count(), Is.EqualTo(0)); // no elements should appear more than once
            Assert.That(groupedById.Where(x => x.Count == 1).Count(), Is.EqualTo(n)); // all elements must appear only once
        }

        [Test]
        public void Error_GivenAnExistingElement_SetOnErrorWithTheSpecifiedErrorMessage()
        {
            // ARRANGE
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(base.ConnectionString);
            using IXfsmDatabaseConnection connection = provider.GetConnection();
            XfsmBag<Sample> xfsm = new XfsmBag<Sample>(provider, XfsmPeekMode.Queue);
            DateTimeOffset updated = "2023-08-14T14:08:16.000+01:00".ToDateTimeOffset();
            DateTimeProvider.Set(updated);
            string message = "Error message";

            xfsm.AddElement(new Sample { Id = 1 }, SampleEnum.State1);
            IXfsmElement<Sample> element = xfsm.Peek(SampleEnum.State1);

            // ACT
            xfsm.Error(element, message);

            // ASSERT
            int count = connection.QueryFirst<int>($"select count(*) from dbo.XfsmElement where id = @id;", new XfsmDatabaseParameter("id", element.GetId()));
            int state = connection.QueryFirst<int>($"select PeekStatus from dbo.XfsmElement where id = @id;", new XfsmDatabaseParameter("id", element.GetId()));
            string resultMessage = connection.QueryFirst<string>($"select Error from dbo.XfsmElement where id = @id;", new XfsmDatabaseParameter("id", element.GetId()));
            DateTimeOffset updateTs = connection.QueryFirst<DateTimeOffset>($"select UpdatedTimeStamp from dbo.XfsmElement where id = @id;", new XfsmDatabaseParameter("id", element.GetId()));
            Assert.That(count, Is.EqualTo(1));
            Assert.That(state, Is.EqualTo((int)XfsmPeekStatus.Error));
            Assert.That(resultMessage, Is.EqualTo(message));
            Assert.That(updateTs, Is.EqualTo(updated));
        }

        [Test]
        public void Error_GivenNonExistingElement_SetOnErrorDoesntThrowAnyException()
        {
            // ARRANGE
            XfsmDatabaseProvider provider = new XfsmDatabaseProvider(base.ConnectionString);
            XfsmBag<Sample> xfsm = new XfsmBag<Sample>(provider, XfsmPeekMode.Queue);
            var mock = new Mock<IXfsmElement<Sample>>();
            mock.Setup(x => x.GetId()).Returns(-123241);
            IXfsmElement<Sample> element = mock.Object;

            // ACT
            xfsm.Error(element, "message");

            // ASSERT
            // no assertion
        }
    }
}