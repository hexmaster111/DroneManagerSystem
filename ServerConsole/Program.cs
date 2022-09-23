using IConsoleLogInterface;

namespace ServerConsole // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        public static ConsoleLogging.ConsoleLog ConsoleLog { get; } = new();
        private static CommandLineHandler commandLineHandler;
        
        
        static void Main(string[] args)
        {
            Console.Title = "Drone management console";
            ConsoleLog.StartLogWriter();
            commandLineHandler = new CommandLineHandler(ConsoleLog);
            commandLineHandler.StartReadThread();
            ConsoleLog.WriteLog(message:"Starting server...");
            ServerBackend.ServerBackend.ConsoleLog = ConsoleLog;
            ServerBackend.ServerBackend.Instance.Start("127.0.0.1", 8000);
        }
    }
}