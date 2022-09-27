using ConsoleCommandHandler.Commands;
using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.ServerInterface;
using GenericMessaging;

namespace ServerConsole.ServerCommands.TestMessages;

public class CSendTestHandshake : ICommand
{
    public string Name => "debug_send_handshake";
    public string[]? Aliases => new[] { "dsh" };
    public string Description => "Sends a test handshake to the client";
    public string RuntimeAssignedNamespace { get; set; } 
    public Argument[]? Arguments => null;
    public ICommandManager CommandManager { get; set; }

    public void Execute(string?[] args, out string? output, out string? errorString, out string? changeToNamespace)
    {
        changeToNamespace = null;
        errorString = null;
        output = null;
        
        try
        {
            ServerBackend.ServerBackend.Instance.Clients[0].SendData(new SendableTarget(
            new HandShakeMessage(new DroneId(DroneType.Experimental, 5050)),
            "HandShake"));
        }
        catch (Exception e)
        {
            output = null;
            errorString = "Exception: " + e.Message;
            return;
        }
        output = "Sent handshake";
    }
}