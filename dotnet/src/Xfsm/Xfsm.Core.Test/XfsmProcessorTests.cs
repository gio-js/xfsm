using Moq;
using Xfsm.Core.Interfaces;

namespace Xfsm.Core.Test
{
    public class XfsmProcessorTests
    {
        internal class Simple
        {
            public Simple() { }
            public Simple(int id) : this()
            {
                this.Id = id;
            }
            public int Id { get; set; }
        }

        internal enum StateEnum
        {
            State1
        }

        [Test]
        public void CanInstantiateObject()
        {
            // ARRANGE
            IXfsmBag<Simple> xfsm = new Mock<IXfsmBag<Simple>>().Object;

            // ACT
            XfsmProcessor<Simple> processor = null; //new XfsmProcessor<Simple>(xfsm);

            // ASSERT
            Assert.That(processor, Is.Not.Null);
        }

        [Test]
        public void CanInstantiateObject_GivenEmptyOrNull_ThrowsNullArgEx()
        {
            // ACT
            //Assert.Throws<ArgumentNullException>(() => new XfsmProcessor<Simple>(null));
        }

        // scenario: no elements in the bag, the process waits for the amount of time specified
        // scenario: 100 elements in the bag, 10 is the max elements to elaborate, the processor only process 10 times
        // scenario: 3 elements, 1 of state1 and 2 for state2, the processor calls the execute on state 1 once, and for state 2 two times


        [Test]
        [Description("3 elements, 1 of state A and 2 for state B, the processor built on state A, calls the execute on state A once")]
        public void WaitAndProcessElement_Given3Elements_1StateA_2StateB_ProcessorCalls_1TimeExecuteOnStateA_And_2TimesExecuteOnStateB()
        {
            // ARRANGE
            var xfsm = new Mock<IXfsmBag<Simple>>();
            var state = new Mock<IXfsmState<Simple>>();
            var el1 = new Mock<IXfsmElement<Simple>>();
            var el2 = new Mock<IXfsmElement<Simple>>();
            var el3 = new Mock<IXfsmElement<Simple>>();
            XfsmProcessor<Simple> processor = new XfsmProcessor<Simple>(xfsm.Object, state.Object);

            xfsm.SetupSequence(x => x.Peek(It.IsAny<Enum>())).Returns(el1.Object).Returns(el2.Object).Returns(el3.Object);

            // ACT
            //processor.WaitAndProcessElement(StateEnum.State1, 10, TimeSpan.FromSeconds(30));

            // ASSERT

        }

    }
}