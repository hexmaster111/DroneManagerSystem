using System.Text;

namespace ConsoleCommandHandler.Commands.RootNamespace;

public class CHelp : ICommand
{
    public string Name => "help";
    public string[]? Aliases => null;
    public string Description => "Runs this help command";
    public string RuntimeAssignedNamespace { get; set; }
    public Argument[]? Arguments => null;
    public ICommandManager CommandManager { get; set; }

    public void Execute(string?[] args, out string? output, out string? errorString, out string? changeToNamespace)
    {
        output = null;
        errorString = null;
        changeToNamespace = null;
        var sb = new StringBuilder();
        sb.AppendLine("Available commands:");
        foreach (var command in CommandManager.Commands)
        {
            sb.AppendLine($"{command.RuntimeAssignedNamespace}.{command.Name}");
            sb.AppendLine($"    {command.Description}");
            sb.AppendLine($"    Aliases: {string.Join(", ", command.Aliases ?? Array.Empty<string>())}");
            if (command.Arguments == null) continue;
            sb.AppendLine($"    Arguments:");
            foreach (var argument in command.Arguments)
                sb.AppendLine($"        {argument.Name} - {argument.Description}");
        }

        output = sb.ToString();
    }
}