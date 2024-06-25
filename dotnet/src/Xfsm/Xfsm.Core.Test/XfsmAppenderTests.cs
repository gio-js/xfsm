using Moq;
using Xfsm.Core.Interfaces;

namespace Xfsm.Core.Test
{
    public class XfsmAppenderTests
    {
        public class SimpleDto
        {
            public int Id { get; set; }
        }

        public enum State
        {
            State1
        }

        [Test]
        public void CanInstantiateObject()
        {
            // ARRANGE
            IXfsmBag<SimpleDto> xfsm = new Mock<IXfsmBag<SimpleDto>>().Object;

            // ACT
            XfsmAppender<SimpleDto> processor = new XfsmAppender<SimpleDto>(xfsm);

            // ASSERT
            Assert.That(processor, Is.Not.Null);
        }

        [Test]
        public void CanInstantiateObject_GivenEmptyOrNull_ThrowsNullArgEx()
        {
            // ACT
            Assert.Throws<ArgumentNullException>(() => new XfsmAppender<SimpleDto>(null));
        }

        [Test]
        [Description("given 1 element, the appender adds the element in the bag")]
        public void WaitAndProcessElement_Given3Elements_InState1_ProcessorCalls_3TimesExecute()
        {
            // ARRANGE
            var xfsm = new Mock<IXfsmBag<SimpleDto>>(MockBehavior.Strict);
            var appender = new XfsmAppender<SimpleDto>(xfsm.Object);

            xfsm.Setup(x => x.AddElement(It.IsAny<SimpleDto>(), It.IsAny<Enum>())).Returns(47239L);

            // ACT
            appender.Add(new SimpleDto { Id = 23948203 }, State.State1);

            // ASSERT
            xfsm.Verify(x => x.AddElement(It.Is<SimpleDto>(x => x.Id == 23948203), It.Is<Enum>(s => s.Equals(State.State1))), Times.Once());
        }
    }
}
