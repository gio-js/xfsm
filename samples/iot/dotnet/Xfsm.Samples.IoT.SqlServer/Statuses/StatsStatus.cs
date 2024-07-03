using Xfsm.Core.Interfaces;
using Xfsm.Samples.IoT.SqlServer.Enums;
using Xfsm.Samples.IoT.SqlServer.Model;
using Xfsm.Samples.IoT.SqlServer.Utils;

namespace Xfsm.Samples.IoT.SqlServer.Statuses
{
    public class StatsStatus : IXfsmState<Measure>
    {
        public void Execute(Measure businessElement, IXfsmStateContext context)
        {
            Logger.LogAction(businessElement, StatusEnum.Stats);
            Logger.Log(StatusEnum.Stats, "Let's calculate statistics on events.");
        }

        public Enum StateEnum()
        {
            return StatusEnum.Stats;
        }
    }
}
