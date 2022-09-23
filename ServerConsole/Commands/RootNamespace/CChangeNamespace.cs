namespace ServerConsole.Commands.RootNamespace;

public class CChangeNamespace : ICommand
{
    public string Name => "ChangeNamespace";
    public string[]? Aliases => new[] { "cn" };
    public string Description => "Changes the current namespace";
    public string RuntimeAssignedNamespace { get; set; }
    public Argument[]? Arguments => new[] { new Argument("namespace", "The namespace to change to") };
    public ICommandManager CommandManager { get; set; }

    public void Execute(string?[] args, out string? output, out string? errorString, out string? changeToNamespace)
    {
        changeToNamespace = null;
        output = null;
        errorString = null;

        if (args.Length <= 1)
        {
            errorString = "Too Few Arguments";
            return;
        }

        if (CommandManager.Namespaces.Contains(args[1]))
        {
            changeToNamespace = args[1];
            output = $"Changed namespace to {args[1]}";
        }
        else if (args[1] == "." || args[1] == "/")
        {
            changeToNamespace = " ";
            output = "Changed namespace to root";
        }
        else if (args[1] == "..")
        {
            if (CommandManager.CurrentNamespace.Length <= 2)
            {
                errorString = "Cannot go up from root";
                return;
            }

            //remove the last item in the current namespace
            var split = CommandManager.CurrentNamespace.Split('.');
            var newNamespace = string.Join(".", split.Take(split.Length - 1));
            changeToNamespace = newNamespace;
            output = $"Changed namespace to {newNamespace}";
        }
        else
            errorString = "Namespace not found";
    }
}