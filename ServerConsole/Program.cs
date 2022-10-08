using System.Reflection;
using ConsoleCommandHandler;
using ServerBackend;
using ServerConsole.ServerCommands;

namespace ServerConsole // Note: actual namespace depends on the project name.
{
    public static class Program
    {
        public static ConsoleLog.ConsoleLog ConsoleLog { get; } = new();
        private static CommandLineHandler commandLineHandler;
        private static RemoteClientManager RemoteClientManager;
        private static ServerBackend.ServerBackend _serverBackend = ServerBackend.ServerBackend.Instance;
        private static DroneClientCommandBuilder droneClientCommandBuilder;

        public static void Main(string[] args)
        {

            Console.Title = "Drone management console";
            ConsoleLog.StartLogWriter();
            commandLineHandler = new CommandLineHandler(ConsoleLog, "ConsoleCommandHandler.Commands.RootNamespace",
                new[] { "ServerConsole.ServerCommands" }, Assembly.GetExecutingAssembly());
            commandLineHandler.StartReadThread();
            ConsoleLog.WriteLog(message: "Starting server...");
            ServerBackend.ServerBackend.ConsoleLog = ConsoleLog;
            // _serverBackend.Start("192.168.1.19", 5000, commandLineHandler);
            _serverBackend.Start("127.0.0.1", 5000, commandLineHandler);
            RemoteClientManager = new RemoteClientManager(_serverBackend, ConsoleLog);
            droneClientCommandBuilder = new DroneClientCommandBuilder(RemoteClientManager, commandLineHandler);
        }
    }
}