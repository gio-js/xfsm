using Xfsm.Core.Interfaces;
using Xfsm.Samples.IoT.SqlServer.Enums;
using Xfsm.Samples.IoT.SqlServer.Model;
using Xfsm.Samples.IoT.SqlServer.Utils;

namespace Xfsm.Samples.IoT.SqlServer.Statuses
{
    public class StoreStatus : IXfsmState<Measure>
    {
        public void Execute(Measure businessElement, IXfsmStateContext context)
        {
            Logger.LogAction(businessElement, StatusEnum.Store);
            Logger.Log(StatusEnum.Store, "Store event in whatever database.");

            // change status
            context.ChangeState(StatusEnum.Stats);
        }

        public Enum StateEnum()
        {
            return StatusEnum.Store;
        }
    }
}
