using Xfsm.Core.Interfaces;
using Xfsm.Samples.IoT.SqlServer.Enums;
using Xfsm.Samples.IoT.SqlServer.Model;
using Xfsm.Samples.IoT.SqlServer.Utils;

namespace Xfsm.Samples.IoT.SqlServer.Statuses
{
    public class AnalyzeStatus : IXfsmState<Measure>
    {
        public void Execute(Measure businessElement, IXfsmStateContext context)
        {
            Logger.LogAction(businessElement, StatusEnum.Analyze);
            if (businessElement.Temperature > 27)
                Logger.Log(StatusEnum.Analyze, "Activate air conditioner");

            if (businessElement.Humidity > 70)
                Logger.Log(StatusEnum.Analyze, "Activate dehumidifier");

            // change status
            context.ChangeState(StatusEnum.Store);
        }

        public Enum StateEnum()
        {
            return StatusEnum.Analyze;
        }
    }
}
