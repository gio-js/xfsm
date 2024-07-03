using Xfsm.Core;
using Xfsm.Core.Enums;
using Xfsm.Samples.IoT.SqlServer.Model;
using Xfsm.Samples.IoT.SqlServer.Statuses;
using Xfsm.SqlServer.Builders;

namespace Xfsm.Samples.IoT.SqlServer.Actors
{
    public class ConsumerProcessorActor : IActor
    {
        private XfsmProcessor<Measure> processorAnalyze;
        private XfsmProcessor<Measure> processorStore;
        private XfsmProcessor<Measure> processorStats;

        public ConsumerProcessorActor(string connectionString)
        {
            this.processorAnalyze = XfsmBuilder.BuildProcessor(connectionString, XfsmPeekMode.Queue, new AnalyzeStatus());
            this.processorStore = XfsmBuilder.BuildProcessor(connectionString, XfsmPeekMode.Queue, new StoreStatus());
            this.processorStats = XfsmBuilder.BuildProcessor(connectionString, XfsmPeekMode.Queue, new StatsStatus());
        }

        public void Run()
        {
            Parallel.For(0, 3, index =>
            {
                switch (index)
                {
                    case 0: processorAnalyze.WaitAndProcessElement(); break;
                    case 1: processorStore.WaitAndProcessElement(); break;
                    case 2: processorStats.WaitAndProcessElement(); break;
                }
            });
        }
    }
}
