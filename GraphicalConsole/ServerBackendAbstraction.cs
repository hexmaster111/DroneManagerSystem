using System;
using System.Reflection;
using ConsoleCommandHandler;
using HaileysHelpers;
using ServerBackend;
using ServerConsole;

namespace GraphicalConsole;

public static class ServerBackendAbstraction
{
    public static ConsoleLog.ConsoleLog ConsoleLog { get; } = new();
    private static CommandLineHandler commandLineHandler;
    private static RemoteClientManager RemoteClientManager;
    private static ServerBackend.ServerBackend _serverBackend = ServerBackend.ServerBackend.Instance;
    private static DroneClientCommandBuilder droneClientCommandBuilder;

    public static IRemoteClientManagerFacade RemoteClientManagerFacade => RemoteClientManager;

    public static void StopServer()
    {
        //TODO: Graceful shutdown
        Environment.Exit(0);
    }

    public static void StartServer()
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