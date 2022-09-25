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
        output = $"Info!";
        errorString = null;
        changeToNamespace = null;
    }
}