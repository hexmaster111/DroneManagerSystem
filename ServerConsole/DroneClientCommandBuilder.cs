using ConsoleCommandHandler;
using ConsoleCommandHandler.Commands;
using DroneManager.Interface.GenericTypes;
using ServerBackend;

namespace ServerConsole;

public class DroneClientCommandBuilder
{
    private IRemoteClientManager _remoteClientManager;
    private ICommandAdder _commandAdder;

    public DroneClientCommandBuilder(IRemoteClientManager remoteClientManager, ICommandAdder commandAdder)
    {
        _remoteClientManager = remoteClientManager;
        _commandAdder = commandAdder;

        remoteClientManager.OnConnectedClient += OnConnectedClient;
    }

    private void OnConnectedClient(DroneClient obj)
    {
        foreach (var command in BuildCommands(obj))
        {
            _commandAdder.AddCommand(command);
        }
    }

    private List<ICommand> BuildCommands(DroneClient client)
    {
        var commands = new List<ICommand>();
        commands.Add(new DroneClientCommandGetInfo(client));

        return commands;
    }

    #region Drone Client Commands

    private class DroneClientCommandGetInfo : ICommand
    {
        private DroneClient _client;

        public DroneClientCommandGetInfo(DroneClient client)
        {
            _client = client;
        }

        public string Name => "getinfo";
        public string[]? Aliases { get; }
        public string Description => "Gets information about the connected client";

        public string RuntimeAssignedNamespace
        {
            get => $"Drones.{_client.Id}";
            set => throw new NotImplementedException("This property is read only");
        }

        public Argument[]? Arguments => null;
        public ICommandManager CommandManager { get; set; }

        public void Execute(string?[] args, out string? output, out string? errorString, out string? changeToNamespace)
        {
            errorString = null;
            changeToNamespace = null;

            output = $"{Environment.NewLine}" +
                     $"ID: {_client.Id}" + Environment.NewLine +
                     $"Health Status: \r\n" +
                     $"     Temperature: {_client.Vitals.Temperature}" + Environment.NewLine +
                     $"     Breathing Rate: {_client.Vitals.BreathingRate}" + Environment.NewLine +
                     $"     Heart Rate: {_client.Vitals.HeartRate}" + Environment.NewLine;

        }
    }

    #endregion
}