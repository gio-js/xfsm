## IoT Sample
A temperature/humidity sensor sends his meaures to a server application who's adopting Xfsm library.

The server runs a processor on a thread specific for every status of the state machine.
In this case we want at least three statuses:
* analyze: in this state, the business logic, will analyze sensor measurement and take the proper actions (i.e. activate air conditionar of heating, etc...)
* store: another process thread will denormalize and store data in a flat table
* stats: the last thread will update statistics based on the stored data

Here a basic picture of the linear and simple process:

![alt text](https://github.com/gio-js/xfsm/blob/main/samples/iot/Xfsm-IOTSample.drawio.png?raw=true)

```
  // define enum status
  public enum StatusEnum
  {
      Analyze,
      Store,
      Stats
  }

  // define the business DTO
  public class Measure
  {
    public double Temperature { get; set; }
    ...
  }

  // implement for every status a proper business logic
  public class AnalyzeStatus : IXfsmState<Measure>
  {
    public void Execute(Measure businessElement, IXfsmStateContext context) { /* business logic here */ }
    public Enum StateEnum() { return StatusEnum.Analyze; }
  }

  public class StoreStatus : IXfsmState<Measure>
  {
    public void Execute(Measure businessElement, IXfsmStateContext context) { /* business logic here */ }
    public Enum StateEnum() { return StatusEnum.Store; }
  }

  public class StatsStatus : IXfsmState<Measure>
  {
    public void Execute(Measure businessElement, IXfsmStateContext context) { /* business logic here */ }
    public Enum StateEnum() { return StatusEnum.Stats; }
  }

  // on the sensor side, create an appender and use it to push data
  XfsmAppender<Measure> appender = XfsmBuilder.BuildAppender<Measure>(connectionString, mode);
  appender.Add(new Measure { ... }, StatesEnum.Analyze);

  // on the server side, create at least one processor for every status
  XfsmProcessor<Measure> processorAnalyze = XfsmBuilder.BuildProcessor(connectionString, mode, new AnalyzeStatus());
  XfsmProcessor<Measure> processorStore = XfsmBuilder.BuildProcessor(connectionString, mode, new StoreStatus());
  XfsmProcessor<Measure> processorStats = XfsmBuilder.BuildProcessor(connectionString, mode, new StatsStatus());

  // run on the server side the processor in different threads
  Parallel.For(0, 3, index =>
  {
      switch (index)
      {
          case 0: processorAnalyze.WaitAndProcessElement(); break;
          case 1: processorStore.WaitAndProcessElement(); break;
          case 1: processorStats.WaitAndProcessElement(); break;
      }
  });

```
