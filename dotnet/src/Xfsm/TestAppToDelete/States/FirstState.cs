using TestAppToDelete.Model;
using Xfsm.Core.Interfaces;

namespace TestAppToDelete.States
{
    public class FirstState : IXfsmState<SampleBusinessElement>
    {
        public void Execute(SampleBusinessElement businessElement, IXfsmStateContext context)
        {
            throw new NotImplementedException();
        }

        public Enum StateEnum()
        {
            return StatesEnum.State1;
        }
    }
}
