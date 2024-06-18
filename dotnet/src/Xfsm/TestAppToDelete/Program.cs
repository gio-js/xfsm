// See https://aka.ms/new-console-template for more information
using TestAppToDelete.States;
using Xfsm.Core.Enums;
using Xfsm.SqlServer.Builders;

Console.WriteLine("Hello, World!");

string connectionString = "";
XfsmPeekMode mode = XfsmPeekMode.Queue;

var processorState1 = XfsmProcessorBuilder.Build(connectionString, mode, new FirstState());
var processorState2 = XfsmProcessorBuilder.Build(connectionString, mode, new SecondState());

Parallel.For(0, 2, index =>
{
    switch (index)
    {
        case 0: processorState1.WaitAndProcessElement(); break;
        case 1: processorState2.WaitAndProcessElement(); break;
    }
});