using Xfsm.Core;
using Xfsm.Samples.IoT.SqlServer.Enums;
using Xfsm.Samples.IoT.SqlServer.Model;

namespace Xfsm.Samples.IoT.SqlServer.Actors
{
    public class SensorDeviceActor : IActor
    {
        private XfsmAppender<Measure> appender = null;
        private Random random = null;

        public SensorDeviceActor(string connectionString)
        {
            this.appender = Xfsm.SqlServer.Builders.XfsmBuilder.BuildAppender<Measure>(connectionString, Core.Enums.XfsmPeekMode.Queue);
            this.random = new Random();
        }

        public void Run()
        {
            do
            {
                // wait a little bit
                Task.Delay(500 + Convert.ToInt16((random.NextInt64() % 1000))).GetAwaiter().GetResult();

                // append new random event
                this.appender.Add(new Measure
                {
                    Temperature = 23 + (random.NextInt64() % 10),
                    Humidity = 40 + (random.NextInt64() % 40),
                }, StatusEnum.Analyze);

            } while (true);

        }
    }
}
