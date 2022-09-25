namespace ServerConsole.Commands.RootNamespace;

public class CEcho : ICommand
{
    public string Name => "echo";
    public string[]? Aliases => null;
    public string Description => "Echos what the user entered as arg1 back to the user";
    public string RuntimeAssignedNamespace { get; set; }
    public Argument[]? Arguments => new[] { new Argument("input", "What will be echoed back to the console") };
    public ICommandManager CommandManager { get; set; }

    public void Execute(string?[] args, out string? output, out string? errorString, out string? changeToNamespace)
    {

        output = null;
        errorString = null;
        changeToNamespace = null;
        
        if (args.Length > 2)
        {
            errorString = "No input string";
            return;
        }


        output = args[1];
    }
}