namespace ServerConsole.Commands;

public interface ICommandManager
{
    public ICommand[] Commands { get; }
    public string[] Namespaces { get; }
    public string CurrentNamespace { get; }
}