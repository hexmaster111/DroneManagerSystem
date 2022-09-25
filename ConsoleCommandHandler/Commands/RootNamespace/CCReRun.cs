namespace ConsoleCommandHandler.Commands.RootNamespace;

public class CCReRun : ICommand
{
    public string Name => "rerun";
    public string[]? Aliases => new []{"!!"};
    public string Description => "Re-runs the last command";
    public string RuntimeAssignedNamespace { get; set; }
    public Argument[]? Arguments => null;
    public ICommandManager CommandManager { get; set; }
    public void Execute(string?[] args, out string? output, out string? errorString, out string? changeToNamespace)
    {
        output = null;
        errorString = null;
        changeToNamespace = null;
        if (CommandManager.CommandHistory == null || CommandManager.CommandHistory.Length == 0)
        {
            errorString = "No commands have been run yet";
            return;
        }


        
        var a = CommandManager.CommandHistory[CommandManager.CommandHistory.Length - 2];
        if(a == "!!") return;
        CommandManager.ExecuteCommand(a);
    }
}