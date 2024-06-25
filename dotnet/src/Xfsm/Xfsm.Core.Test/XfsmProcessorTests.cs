using Moq;
using Xfsm.Core.Interfaces;
using Xfsm.Core.Test.Builder;

namespace Xfsm.Core.Test
{
    public class XfsmProcessorTests
    {
        public class Simple
        {
            public Simple() { }
            public Simple(int id) : this()
            {
                this.Id = id;
            }
            public int Id { get; set; }
        }

        public enum StateEnum
        {
            State1
        }

        [Test]
        public void CanInstantiateObject()
        {
            // ARRANGE
            IXfsmBag<Simple> xfsm = new Mock<IXfsmBag<Simple>>().Object;
            IXfsmState<Simple> state = new Mock<IXfsmState<Simple>>(MockBehavior.Strict).Object;

            // ACT
            XfsmProcessor<Simple> processor = new XfsmProcessor<Simple>(xfsm, state);

            // ASSERT
            Assert.That(processor, Is.Not.Null);
        }

        [Test]
        public void CanInstantiateObject_GivenEmptyOrNull_ThrowsNullArgEx()
        {
            // ACT
            Assert.Throws<ArgumentNullException>(() => new XfsmProcessor<Simple>(null, null));
        }

        [Test]
        [Description("3 elements, the processor calls the execute on state 1 three times")]
        public void WaitAndProcessElement_Given3Elements_InState1_ProcessorCalls_3TimesExecute()
        {
            // ARRANGE
            var xfsm = new Mock<IXfsmBag<Simple>>(MockBehavior.Strict);
            var state = new Mock<IXfsmState<Simple>>(MockBehavior.Strict);
            var el1 = Element<Simple>.Build().With(x => x.State, StateEnum.State1).With(x => x.BusinessElement, new Simple(1));
            var el2 = Element<Simple>.Build().With(x => x.State, StateEnum.State1).With(x => x.BusinessElement, new Simple(2));
            var el3 = Element<Simple>.Build().With(x => x.State, StateEnum.State1).With(x => x.BusinessElement, new Simple(3));
            var doExecute = 0;
            var processor = new XfsmProcessor<Simple>(xfsm.Object, state.Object, 3, TimeSpan.FromSeconds(1));
            xfsm.SetupSequence(x => x.Peek(It.IsAny<Enum>())).Returns(el1).Returns(el2).Returns(el3);
            state.Setup(s => s.StateEnum()).Returns(StateEnum.State1);
            state.Setup(s => s.Execute(It.IsAny<Simple>(), It.IsAny<IXfsmStateContext>())).Callback(() => { doExecute++; }).Verifiable();

            // ACT
            processor.WaitAndProcessElement();

            // ASSERT
            Assert.That(doExecute, Is.EqualTo(3));
            state.Verify(x => x.Execute(It.Is<Simple>(x => x.Id == 1), It.IsAny<IXfsmStateContext>()), Times.Once());
            state.Verify(x => x.Execute(It.Is<Simple>(x => x.Id == 2), It.IsAny<IXfsmStateContext>()), Times.Once());
            state.Verify(x => x.Execute(It.Is<Simple>(x => x.Id == 3), It.IsAny<IXfsmStateContext>()), Times.Once());
            //xfsm.Verify(x => x.Done(It.Is<IXfsmElement<Simple>>(x => x.GetId() == el1.Id)), Times.Once());
            //xfsm.Verify(x => x.Done(It.Is<IXfsmElement<Simple>>(x => x.GetId() == el2.Id)), Times.Once());
            //xfsm.Verify(x => x.Done(It.Is<IXfsmElement<Simple>>(x => x.GetId() == el3.Id)), Times.Once());
        }

        [Test]
        [Description("no elements in the bag, the process waits for the amount of time specified")]
        public void WaitAndProcessElement_NoElements_ProcessorWaitsForTheAmountOfTime()
        {
            // ARRANGE
            var xfsm = new Mock<IXfsmBag<Simple>>(MockBehavior.Strict);
            var state = new Mock<IXfsmState<Simple>>(MockBehavior.Strict);
            var now = DateTime.Now;
            var maximumElapsedTime = TimeSpan.FromSeconds(2);
            var processor = new XfsmProcessor<Simple>(xfsm.Object, state.Object, 3, maximumElapsedTime);
            xfsm.SetupSequence(x => x.Peek(It.IsAny<Enum>())).Returns((IXfsmElement<Simple>)null);
            state.Setup(s => s.StateEnum()).Returns(StateEnum.State1);

            // ACT
            processor.WaitAndProcessElement();

            // ASSERT
            Assert.That(DateTime.Now.Subtract(now), Is.EqualTo(maximumElapsedTime).Within(TimeSpan.FromMilliseconds(500)));
        }

        [Test]
        [Description("3 elements, 2 is the maximum elements to elaborate, the processor calls the execute on state 1 only two times")]
        public void WaitAndProcessElement_Given3Elements_MaximumToElaborateIs2_ProcessorCalls_2TimesExecute()
        {
            // ARRANGE
            var xfsm = new Mock<IXfsmBag<Simple>>(MockBehavior.Strict);
            var state = new Mock<IXfsmState<Simple>>(MockBehavior.Strict);
            var el1 = Element<Simple>.Build().With(x => x.State, StateEnum.State1).With(x => x.BusinessElement, new Simple(1));
            var el2 = Element<Simple>.Build().With(x => x.State, StateEnum.State1).With(x => x.BusinessElement, new Simple(2));
            var el3 = Element<Simple>.Build().With(x => x.State, StateEnum.State1).With(x => x.BusinessElement, new Simple(3));
            var doExecute = 0;
            var processor = new XfsmProcessor<Simple>(xfsm.Object, state.Object, 2, TimeSpan.FromSeconds(1));
            xfsm.SetupSequence(x => x.Peek(It.IsAny<Enum>())).Returns(el1).Returns(el2).Returns(el3);
            state.Setup(s => s.StateEnum()).Returns(StateEnum.State1);
            state.Setup(s => s.Execute(It.IsAny<Simple>(), It.IsAny<IXfsmStateContext>())).Callback(() => { doExecute++; }).Verifiable();

            // ACT
            processor.WaitAndProcessElement();

            // ASSERT
            Assert.That(doExecute, Is.EqualTo(2));
            state.Verify(x => x.Execute(It.Is<Simple>(x => x.Id == 1), It.IsAny<IXfsmStateContext>()), Times.Once());
            state.Verify(x => x.Execute(It.Is<Simple>(x => x.Id == 2), It.IsAny<IXfsmStateContext>()), Times.Once());
            state.Verify(x => x.Execute(It.Is<Simple>(x => x.Id == 3), It.IsAny<IXfsmStateContext>()), Times.Never());
        }

        [Test]
        [Description("1 element, in case of exception, the processor set the element in error status")]
        public void WaitAndProcessElement_Given1Element_TheExecutionThrowsException_ProcessorSetTheElementInError()
        {
            // ARRANGE
            var xfsm = new Mock<IXfsmBag<Simple>>(MockBehavior.Strict);
            var state = new Mock<IXfsmState<Simple>>(MockBehavior.Strict);
            var el1 = Element<Simple>.Build()
                .With(x => x.Id, 1)
                .With(x => x.State, StateEnum.State1)
                .With(x => x.BusinessElement, new Simple(1));
            var processor = new XfsmProcessor<Simple>(xfsm.Object, state.Object, 1, TimeSpan.FromSeconds(1));
            var exceptionMessage = "This is the exception";
            xfsm.Setup(x => x.Peek(It.IsAny<Enum>())).Returns(el1);
            state.Setup(s => s.StateEnum()).Returns(StateEnum.State1);
            state.Setup(s => s.Execute(It.IsAny<Simple>(), It.IsAny<IXfsmStateContext>())).Callback(() => { throw new Exception(exceptionMessage); }).Verifiable();
            xfsm.Setup(s => s.Error(It.IsAny<IXfsmElement<Simple>>(), It.IsAny<string>())).Callback(() => { /* do nothing */ }).Verifiable();

            // ACT
            processor.WaitAndProcessElement();

            // ASSERT
            state.Verify(x => x.Execute(It.Is<Simple>(x => x.Id == 1), It.IsAny<IXfsmStateContext>()), Times.Once());
            xfsm.Verify(x => x.Error(It.Is<IXfsmElement<Simple>>(x => x.GetId() == 1), It.Is<string>(s => s.Contains(exceptionMessage))), Times.Once());
        }

        [Test]
        [Description("3 element, only the second throws exception on execute, the processor set the second element in error status and process all three elements")]
        public void WaitAndProcessElement_Given3Elements_TheSecondOnThrowsException_ProcessorExecuteAlleTheThreeElements_AndSetTheSecondElementInError()
        {
            // ARRANGE
            var xfsm = new Mock<IXfsmBag<Simple>>(MockBehavior.Strict);
            var state = new Mock<IXfsmState<Simple>>(MockBehavior.Strict);
            var el1 = Element<Simple>.Build().With(x => x.Id, 1).With(x => x.BusinessElement, new Simple(1));
            var el2 = Element<Simple>.Build().With(x => x.Id, 2).With(x => x.BusinessElement, new Simple(2));
            var el3 = Element<Simple>.Build().With(x => x.Id, 3).With(x => x.BusinessElement, new Simple(3));
            var processor = new XfsmProcessor<Simple>(xfsm.Object, state.Object, 3, TimeSpan.FromSeconds(3));
            var exceptionMessage = "This is the exception";
            xfsm.SetupSequence(x => x.Peek(It.IsAny<Enum>())).Returns(el1).Returns(el2).Returns(el3);
            state.Setup(s => s.StateEnum()).Returns(StateEnum.State1);
            state.Setup(s => s.Execute(It.IsAny<Simple>(), It.IsAny<IXfsmStateContext>())).Callback((Simple x, IXfsmStateContext y) => { 
                if (x.Id == 2) throw new Exception(exceptionMessage); }).Verifiable();
            xfsm.Setup(s => s.Error(It.IsAny<IXfsmElement<Simple>>(), It.IsAny<string>())).Callback(() => { /* do nothing */ }).Verifiable();

            // ACT
            processor.WaitAndProcessElement();

            // ASSERT
            state.Verify(x => x.Execute(It.Is<Simple>(x => x.Id == 1), It.IsAny<IXfsmStateContext>()), Times.Once());
            state.Verify(x => x.Execute(It.Is<Simple>(x => x.Id == 2), It.IsAny<IXfsmStateContext>()), Times.Once());
            state.Verify(x => x.Execute(It.Is<Simple>(x => x.Id == 3), It.IsAny<IXfsmStateContext>()), Times.Once());
            xfsm.Verify(x => x.Error(It.IsAny<IXfsmElement<Simple>>(), It.IsAny<string>()), Times.Once());
            xfsm.Verify(x => x.Error(It.Is<IXfsmElement<Simple>>(x => x.GetId() == 2), It.Is<string>(s => s.Contains(exceptionMessage))), Times.Once());
        }
    }
}