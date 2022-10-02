using System.Runtime.CompilerServices;
using System.Text;
using ConsoleCommandHandler;
using ConsoleCommandHandler.Commands;
using Contracts.ContractDTOs;
using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.Remote;
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
        remoteClientManager.OnDisconnectedClient += OnDisconnectedClient;
    }

    private void OnDisconnectedClient(DroneClient obj)
    {
        //Build a list from _addedCommands for all commands that have the same id as objs id

        foreach (var command in _addedCommands)
        {
            if (command.RuntimeAssignedNamespace.Contains(obj.Id.ToString()))
            {
                _commandAdder.RemoveCommand(command);
            }
        }
    }


    private List<ICommand> _addedCommands = new();


    private void OnConnectedClient(DroneClient obj)
    {
        foreach (var command in BuildCommands(obj))
        {
            _commandAdder.AddCommand(command);
            _addedCommands.Add(command);
        }
    }

    private List<ICommand> BuildCommands(DroneClient client)
    {
        var commands = new List<ICommand>();
        commands.Add(new CDroneStatus(client));
        commands.Add(new CMessageDrone(client));

        return commands;
    }

    #region Drone Client Commands

    private class CDroneStatus : ICommand
    {
        private DroneClient _client;

        public CDroneStatus(DroneClient client)
        {
            _client = client;
        }

        public string Name => "status";
        public string[]? Aliases { get; }
        public string Description => "Gets information about the connected client";

        public string RuntimeAssignedNamespace
        {
            get => $"Drones.{_client.Id}";
            set => throw new NotImplementedException("This property is read only");
        }

        public Argument[]? Arguments => null;
        public ICommandManager CommandManager { get; set; }

        private bool _checkIfVitalsOutsideOfOkRange(IVital vital)
        {
            if (double.IsNaN(vital.Temperature) || double.IsNaN(vital.BreathingRate) || double.IsNaN(vital.HeartRate))
                return true;

            if (vital.Temperature > vital.MaxTemperature || vital.Temperature < vital.MinTemperature)
            {
                return true;
            }

            if (vital.BreathingRate > vital.MaxBreathingRate || vital.BreathingRate < vital.MinBreathingRate)
            {
                return true;
            }

            if (vital.HeartRate > vital.MaxHeartRate || vital.HeartRate < vital.MinHeartRate)
            {
                return true;
            }

            return false;
        }

        public void Execute(string?[] args, out string? output, out string? errorString, out string? changeToNamespace)
        {
            errorString = null;
            changeToNamespace = null;

            var tab = "\t";

            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine($"Id: {_client.Id} ");
            sb.AppendLine($"{tab}Network Info: ");
            sb.AppendLine(
                $"{tab}{tab}IP: {_client.RemoteClient?.NetworkInformation.ClientProviderAddress}:{_client.RemoteClient?.NetworkInformation.ClientProviderPort}");
            sb.AppendLine($"{tab}{tab}Connected: {_client.RemoteClient?.NetworkInformation.IsConnected}");
            sb.AppendLine($"{tab}{tab}Last Mesg: {_client.RemoteClient?.NetworkInformation.LastMessage}");

            sb.AppendLine($"{tab}Drone Info: ");
            sb.AppendLine($"{tab}{tab}Health Status: " + "todo _client.Vitals.HealthStatus.ToString()");
            if (_checkIfVitalsOutsideOfOkRange(_client.Vitals))
            {
                sb.AppendLine($"{tab}{tab}{tab}Vitals are outside of OK range");
                //print out vitals
                sb.AppendLine(
                    $"{tab}{tab}{tab}{tab}Temperature: {_client.Vitals.Temperature} °C  (Min: {_client.Vitals.MinTemperature} °C, Max: {_client.Vitals.MaxTemperature} °C)");
                sb.AppendLine(
                    $"{tab}{tab}{tab}{tab}Breathing Rate: {_client.Vitals.BreathingRate} bpm  (Min: {_client.Vitals.MinBreathingRate} bpm, Max: {_client.Vitals.MaxBreathingRate} bpm)");
                sb.AppendLine(
                    $"{tab}{tab}{tab}{tab}Heart Rate: {_client.Vitals.HeartRate} bpm  (Min: {_client.Vitals.MinHeartRate} bpm, Max: {_client.Vitals.MaxHeartRate} bpm)");
            }

            if (_client.CurrentLocation != null)
            {
                sb.AppendLine($"{tab}Location: ");
                sb.AppendLine($"{tab}{tab}Latitude: {_client.CurrentLocation.Latitude}");
                sb.AppendLine($"{tab}{tab}Longitude: {_client.CurrentLocation.Longitude}");

                sb.AppendLine($"{tab}{tab}Speed: {_client.CurrentLocation.Speed}");
                sb.AppendLine($"{tab}{tab}Address: {_client.CurrentLocation.LocationAddress}");
                sb.AppendLine($"{tab}{tab}Name: {_client.CurrentLocation.LocationName}");
                sb.AppendLine($"{tab}{tab}Provider: {_client.CurrentLocation.LocationProvider}");
                sb.AppendLine($"{tab}{tab}Last Updated: {_client.CurrentLocation.TimeStamp}");
            }

            output = sb.ToString();
        }
    }

    private class CMessageDrone : ICommand
    {
        private DroneClient _client;

        public CMessageDrone(DroneClient client)
        {
            _client = client;
        }

        public string Name => "sendMessage";
        public string[]? Aliases => new[] { "msg" };
        public string Description => "Sends a message to the drone";

        public string RuntimeAssignedNamespace
        {
            get => $"Drones.{_client.Id}";
            set => throw new NotImplementedException("This property is read only");
        }

        public Argument[]? Arguments => new Argument[]
        {
            new Argument("message", "The message to send to the drone", Argument.CompleteHelperType.None)
        };

        public ICommandManager CommandManager { get; set; }

        public void Execute(string?[] args, out string? output, out string? errorString, out string? changeToNamespace)
        {
            errorString = null;
            changeToNamespace = null;
            output = null;

            if (args == null || args.Length == 0)
            {
                errorString = "No message was provided";
                return;
            }

            var message = string.Join(" ", args);

            if (string.IsNullOrWhiteSpace(message))
            {
                errorString = "No message was provided";
                return;
            }

            _client.RemoteClient.SendingContract.BroadcastChatMessage.Send(new ChatMessage("PM", message));
        }
    }

    #endregion
}