using IConsoleLog;

namespace ServerConsole // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        public static ConsoleLog.ConsoleLog ConsoleLog { get; } = new();
        
        static void Main(string[] args)
        {
            Console.Title = "Drone management console";
            ConsoleLog.StartLogWriter();
            ConsoleLog.WriteLog(message:"Starting server...");
            ServerBackend.ServerBackend.ConsoleLog = ConsoleLog;
            ServerBackend.ServerBackend.Instance.Start("127.0.0.1", 8000);
        }
    }
}