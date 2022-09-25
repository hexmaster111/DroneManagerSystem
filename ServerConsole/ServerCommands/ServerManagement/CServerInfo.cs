using System.Text;
using Server;

namespace ServerConsole.Commands.RootNamespace.ServerManagement;

public class CServerInfo : ICommand
{
    public CServerInfo()
    {
    }

    public string Name => "info";
    public string[]? Aliases => new []{"serverInfo"};
    public string Description => "Displays information about the server.";
    public string RuntimeAssignedNamespace { get; set; }
    
    public Argument[]? Arguments => null;
    
    public ICommandManager CommandManager { get; set; }

    public void Execute(string?[] args, out string? output, out string? errorString,
        out string? changeToNamespace)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Server Information:");
        sb.AppendLine("Server Listener running: " + ServerBackend.Instance.IsRunning);
        sb.AppendLine("Server Internal IP: " + ServerBackend.Instance.LocalIp);
        sb.AppendLine("Client Listener running on port: " + ServerBackend.Instance.ServerPort);
        sb.AppendLine("Clients Connected: " + ServerBackend.Instance.ConnectedClients);
        
        
        output = sb.ToString();
        errorString = null;
        changeToNamespace = null;
        
    }
}