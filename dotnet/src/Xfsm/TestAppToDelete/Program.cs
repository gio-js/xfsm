// See https://aka.ms/new-console-template for more information
using TestAppToDelete.States;
using Xfsm.Core.Enums;
using Xfsm.SqlServer.Builders;

Console.WriteLine("Hello, World!");

string connectionString = "Data Source=localhost,5434;User=sa;Password=Pass@word;Database=master;TrustServerCertificate=true";
XfsmPeekMode mode = XfsmPeekMode.Queue;

var processorState1 = XfsmBuilder.BuildProcessor(connectionString, mode, new FirstState());
var processorState2 = XfsmBuilder.BuildProcessor(connectionString, mode, new SecondState());

Parallel.For(0, 2, index =>
{
    switch (index)
    {
        case 0: processorState1.WaitAndProcessElement(); break;
        case 1: processorState2.WaitAndProcessElement(); break;
    }
});