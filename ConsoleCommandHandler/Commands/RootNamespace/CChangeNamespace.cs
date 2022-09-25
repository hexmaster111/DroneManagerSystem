namespace ConsoleCommandHandler.Commands.RootNamespace;

public class CChangeNamespace : ICommand
{
    public string Name => "ChangeNamespace";
    public string[]? Aliases => new[] { "cn", "cd" };
    public string Description => "Changes the current namespace";
    public string RuntimeAssignedNamespace { get; set; }
    public Argument[]? Arguments => new[] { new Argument("namespace", "The namespace to change to", Argument.CompleteHelperType.Namespace) };
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

        if (CommandManager.Namespaces.Contains(args[1])) //cd to namespace
        {
            changeToNamespace = args[1];
            output = $"Changed namespace to {args[1]}";
        }
        else if (_isWithinThisNamespace(args[1])) //if the name of the new namespace is within this namespace
        {
            changeToNamespace = $"{CommandManager.CurrentNamespace}.{args[1]}";
            output = $"Changed namespace to {changeToNamespace}";
        }
        else
            switch (args[1])
            {
                //up a namespace
                case ".." when CommandManager.CurrentNamespace.Length <= 2:
                    errorString = "Cannot go up from root";
                    return;
                case "..":
                {
                    var split = CommandManager.CurrentNamespace.Split('.');
                    //remove the last item in the current namespace
                    var newNamespace = string.Join(".", split.Take(split.Length - 1));
                    changeToNamespace = newNamespace;
                    output = $"Changed namespace to {newNamespace}";
                    break;
                }
                //ToRoot 
                case "/":
                    changeToNamespace = "";
                    output = $"Changed namespace to root";
                    break;
                default:
                    errorString = "Namespace not found";
                    break;
            }
    }


    private bool _isWithinThisNamespace(string? newNamespaceArg)
    {
        //if the name of the new namespace is within this namespace
        var currentNamespace = CommandManager.CurrentNamespace;

        //Add the new namespace to the current namespace
        var newNamespace = $"{currentNamespace}.{newNamespaceArg}";

        //test if the new namespace is available to change to
        return CommandManager.Namespaces.Contains(newNamespace);
    }
}