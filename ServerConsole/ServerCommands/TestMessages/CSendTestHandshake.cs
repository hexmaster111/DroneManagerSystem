using ConsoleCommandHandler.Commands;
using Contracts.ContractDTOs;
using DroneManager.Interface.GenericTypes;
using GenericEventMapper;
using GenericMessaging;

namespace ServerConsole.ServerCommands.TestMessages;

public class CSendTestHandshake : ICommand
{
    public string Name => "debug_send_handshake";
    public string[]? Aliases => new[] { "dsh" };
    public string Description => "Sends a test handshake to the client";
    public string RuntimeAssignedNamespace { get; set; }

    public Argument[]? Arguments => new[]
        { new Argument("TestCmdNumber", "Debug message to send", Argument.CompleteHelperType.None) };

    public ICommandManager CommandManager { get; set; }

    public void Execute(string?[] args, out string? output, out string? errorString, out string? changeToNamespace)
    {
        changeToNamespace = null;
        errorString = null;
        output = null;

        try
        {
            switch (args[1])
            {
                case "1":
                    ServerBackend.ServerBackend.Instance.Clients[0].SendData(new SendableTarget(
                        new HandShakeMessage(new DroneId(DroneType.Experimental, 5050)),
                        "BadTarget"));
                    output = "Sent badTarget";
                    return;
                case "2":
                    ServerBackend.ServerBackend.Instance.Clients[0].SendData(new SendableTarget(
                        new HandShakeMessage(new DroneId(DroneType.Experimental, 5050)),
                        "HandShake"));
                    output = "Sent handshake";
                    return;
            }
        }
        catch (Exception e)
        {
            output = null;
            errorString = "Exception: " + e.Message;
            return;
        }
        
        output = null;
        errorString = "Invalid argument";
    }
}