using Newtonsoft.Json;
using Xfsm.Samples.IoT.SqlServer.Enums;
using Xfsm.Samples.IoT.SqlServer.Model;

namespace Xfsm.Samples.IoT.SqlServer.Utils
{
    public static class Logger
    {
        public static void LogAction(Measure measure, StatusEnum status)
        {
            DateTime now = DateTime.Now;
            Console.WriteLine($"{now:ddMMyyyyTHHmmssfff}, status: {status}, element: {JsonConvert.SerializeObject(measure)}");
        }
        public static void Log(StatusEnum status, string message)
        {
            DateTime now = DateTime.Now;
            Console.WriteLine($"{now:ddMMyyyyTHHmmssfff}, status: {status}, message: {message}");
        }
    }
}
