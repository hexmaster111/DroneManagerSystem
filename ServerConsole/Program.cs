using System.Reflection;
using ConsoleCommandHandler;
using ServerBackend;

namespace ServerConsole // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        public static ConsoleLog.ConsoleLog ConsoleLog { get; } = new();
        private static CommandLineHandler commandLineHandler;

        private static RemoteClientManager RemoteClientManager;
        private static ServerBackend.ServerBackend _serverBackend = ServerBackend.ServerBackend.Instance;


        static void Main(string[] args)
        {
            Console.Title = "Drone management console";
            ConsoleLog.StartLogWriter();
            commandLineHandler = new CommandLineHandler(ConsoleLog, "ConsoleCommandHandler.Commands.RootNamespace",
                new[] { "ServerConsole.ServerCommands" }, Assembly.GetExecutingAssembly());
            commandLineHandler.StartReadThread();
            ConsoleLog.WriteLog(message: "Starting server...");
            ServerBackend.ServerBackend.ConsoleLog = ConsoleLog;
            _serverBackend.Start("127.0.0.1", 5000, commandLineHandler);
            RemoteClientManager = new RemoteClientManager(_serverBackend, ConsoleLog);
            
        }
    }
}