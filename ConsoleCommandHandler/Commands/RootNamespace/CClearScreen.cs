namespace ConsoleCommandHandler.Commands.RootNamespace;

public class CClearScreen : ICommand
{
    public string Name => "clear";
    public string[]? Aliases => new[] { "cls" };
    public string Description => "Clears the console screen.";
    public string RuntimeAssignedNamespace { get; set; }
    public Argument[]? Arguments => null;
    public ICommandManager CommandManager { get; set; }
    public void Execute(string?[] args, out string? output, out string? errorString, out string? changeToNamespace)
    {
        output = null;
        errorString = null;
        changeToNamespace = null;
        Console.Clear();
    }
}