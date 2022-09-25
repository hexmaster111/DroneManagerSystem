using IConsoleLogInterface;
using Server;

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
            commandLineHandler = new CommandLineHandler(ConsoleLog, "ServerConsole.Commands.RootNamespace",
                new [] { "ServerConsole.ServerCommands" });
            commandLineHandler.StartReadThread();
            ConsoleLog.WriteLog(message: "Starting server...");
            ServerBackend.ConsoleLog = ConsoleLog;
            ServerBackend.Instance.Start("127.0.0.1", 5000);
        }
    }
}