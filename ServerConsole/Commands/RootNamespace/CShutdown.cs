namespace ServerConsole.Commands.RootNamespace;

public class CShutdown : ICommand
{
    public CShutdown(){}
    
    public string Name => "shutdown";
    public string[]? Aliases => null;
    public string Description => "Shuts down the server";
    public string RuntimeAssignedNamespace { get; set; }

    public Argument[]? Arguments => null;
    public ICommandManager CommandManager { get; set; }


    public void Execute(string?[] args, out string? output, out string? errorString,
        out string? changeToNamespace)
    {
        Environment.Exit(0);

        output = null;
        errorString = null;
        changeToNamespace = null;
    }
}