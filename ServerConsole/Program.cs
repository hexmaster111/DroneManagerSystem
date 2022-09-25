using System.Reflection;
using ConsoleCommandHandler;

namespace ServerConsole // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        public static ConsoleLog.ConsoleLog ConsoleLog { get; } = new();
        private static CommandLineHandler commandLineHandler;


        static void Main(string[] args)
        {
            Console.Title = "Drone management console";
            ConsoleLog.StartLogWriter();
            commandLineHandler = new CommandLineHandler(ConsoleLog, "ConsoleCommandHandler.Commands.RootNamespace",
                new [] { "ServerConsole.ServerCommands" }, Assembly.GetExecutingAssembly());
            commandLineHandler.StartReadThread();
            ConsoleLog.WriteLog(message: "Starting server...");
            ServerBackend.ServerBackend.ConsoleLog = ConsoleLog;
            ServerBackend.ServerBackend.Instance.Start("127.0.0.1", 5000);
        }
    }
}