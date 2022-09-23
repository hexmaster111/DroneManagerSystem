namespace ServerConsole.Commands;

public interface ICommand
{
    public string Name { get; }
    public string[]? Aliases { get; }
    public string Description { get; }
    public string RuntimeAssignedNamespace { get; set; }
    public Argument[]? Arguments { get; }
    
    public ICommandManager CommandManager { set; }

    public void Execute(string?[] args, out string? output, out string? errorString,
        out string? changeToNamespace);
}