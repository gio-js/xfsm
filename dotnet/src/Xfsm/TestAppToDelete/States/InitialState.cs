using TestAppToDelete.Model;
using Xfsm.Core.Abstract;

namespace TestAppToDelete.States
{
    public class InitialState : XfsmState<SampleBusinessElement>
    {
        public InitialState(XfsmElement<SampleBusinessElement> context) : base(context) { }

        public override void Execute()
        {
            Console.WriteLine("Initial state");
            base.ChangeState(States.Second); // new SecondState(base.context));
        }

        public override Enum GetStateUniqueValue()
        {
            return States.First;
        }
    }

    public enum States
    {
        First,
        Second
    }
}
