using CommandLine;
using Xfsm.Samples.IoT.SqlServer.Actors;
using Xfsm.Samples.IoT.SqlServer.Model.Options;

var factory = new ActorFactory();

Parser.Default.ParseArguments<CommandLineOptions>(args)
    .WithParsed<CommandLineOptions>(o => factory.Create(o.Command, o.ConnectionString).Run());