using CommandLine;

namespace Xfsm.Samples.IoT.SqlServer.Model.Options
{
    public class CommandLineOptions
    {
        [Option('c', "command", Required = true, HelpText = "Command to be executed: run-sensor, run-consumer")]
        public string Command { get; set; }

        [Option('s', "connection", Required = true, HelpText = "Database connection string")]
        public string ConnectionString { get; set; }
    }
}
