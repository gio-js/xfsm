using System;
using System.Collections.Generic;
using System.Text;

namespace Xfsm.Core.Interfaces
{
    public interface IXfsmStateContext
    {
        void ChangeState(Enum state);
    }
}
