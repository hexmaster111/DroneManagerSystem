using ConsoleCommandHandler.Commands;
using Contracts.ContractDTOs;
using ServerBackend;

namespace ServerConsole.ServerCommands.Drones;

public class CMessageBroadcast : ICommand
{
    public string Name => "broadcast";
    public string[]? Aliases => new []{"bc"};
    public string Description => "Broadcast a message to all connected clients";
    public string RuntimeAssignedNamespace { get; set; } = "Drones";
    public Argument[]? Arguments => new Argument[]
    {
        new Argument("message", "The message to broadcast", Argument.CompleteHelperType.None)
    };
    public ICommandManager CommandManager { get; set; }
    public void Execute(string?[] args, out string? output, out string? errorString, out string? changeToNamespace)
    {
        changeToNamespace = null;
        errorString = null;
        output = null;

        if (args == null || args.Length == 0)
        {
            errorString = "No message specified";
            return;
        }

        var message = string.Join(" ", args);

        foreach (var client in ServerBackend.ServerBackend.Instance.Clients)
        {
            client.SendingContract.BroadcastChatMessage.Send(new ChatMessage("Console", message));
        }
    }
}