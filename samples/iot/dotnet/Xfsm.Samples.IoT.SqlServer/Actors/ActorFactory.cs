namespace Xfsm.Samples.IoT.SqlServer.Actors
{
    public class ActorFactory
    {
        public IActor Create(string command, string connectionString)
        {
            switch (command)
            {
                case "run-sensor": return new SensorDeviceActor(connectionString);
                case "run-consumer": return new ConsumerProcessorActor(connectionString);
                default:
                    throw new ArgumentException($"{command} is not supported.");
            }
        }
    }
}
