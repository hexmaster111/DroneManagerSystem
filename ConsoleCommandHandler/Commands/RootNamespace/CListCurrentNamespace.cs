using System.Text;

namespace ConsoleCommandHandler.Commands.RootNamespace;

public class CListCurrentNamespace : ICommand
{
    public string Name => "ListCurrentNamespace";
    public string[]? Aliases => new[] { "lcns", "ls" };
    public string Description => "Lists the current namespace and items in it.";
    public string RuntimeAssignedNamespace { get; set; }
    public Argument[]? Arguments => null;
    public ICommandManager CommandManager { get; set; }

    public void Execute(string?[] args, out string? output, out string? errorString, out string? changeToNamespace)
    {
        output = null;
        errorString = null;
        changeToNamespace = null;


        var sb = new StringBuilder();

        sb.AppendLine("Available Namespaces:");
        
        foreach (var command in CommandManager.Commands)
        {
            if (!sb.ToString().Contains(command.RuntimeAssignedNamespace))
            {
                sb.AppendLine("         " + command.RuntimeAssignedNamespace);
            }
        }

        // sb.AppendLine($"Current namespace: {CommandManager.CurrentNamespace}");
        sb.AppendLine($"Items in namespace: {CommandManager.CurrentNamespace}");
        foreach (var command in CommandManager.Commands)
        {
            if (command.RuntimeAssignedNamespace.Equals(CommandManager.CurrentNamespace))
                sb.AppendLine("     " + command.Name);
        }

        output = sb.ToString();
    }
}