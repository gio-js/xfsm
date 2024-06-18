﻿using TestAppToDelete.Model;
using Xfsm.Core.Interfaces;

namespace TestAppToDelete.States
{
    public class SecondState : IXfsmState<SampleBusinessElement>
    {
        public void Execute(SampleBusinessElement businessElement, IXfsmStateContext context)
        {
            throw new NotImplementedException();
        }

        public Enum StateEnum()
        {
            return StatesEnum.State2;
        }
    }
}
