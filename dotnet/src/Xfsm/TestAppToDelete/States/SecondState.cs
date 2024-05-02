using TestAppToDelete.Model;
using Xfsm.Core.Abstract;

namespace TestAppToDelete.States
{
    public class SecondState : XfsmState<SampleBusinessElement>
    {
        public SecondState(XfsmElement<SampleBusinessElement> context) : base(context) { }

        public override void Execute()
        { }

        public override Enum GetStateUniqueValue()
        {
            throw new NotImplementedException();
        }
    }
}
