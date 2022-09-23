namespace ServerConsole.Commands.RootNamespace;

public class CListNamespace : ICommand
{
    public string Name => "ListAllNamespaces";
    public string[]? Aliases => new[] { "ns" };

    public string Description => "List all namespaces";
    public string RuntimeAssignedNamespace { get; set; }
    public Argument[]? Arguments => null;
    public ICommandManager CommandManager { get; set; }

    public void Execute(string?[] args, out string? output, out string? errorString, out string? changeToNamespace)
    {
        output = "Current namespace: " + CommandManager.CurrentNamespace + "\r\n";
        output += "     Available Namespaces:\r\n";
        foreach (var command in CommandManager.Commands)
        {
            if (!output.Contains(command.RuntimeAssignedNamespace))
            {
                output += "         " + command.RuntimeAssignedNamespace;
            }
        }

        errorString = null;
        changeToNamespace = null;
    }
}