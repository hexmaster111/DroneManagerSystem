namespace ConsoleCommandHandler.Commands;

public interface ICommandManager
{
    public void ExecuteCommand(string command);
    
    public ICommand[] Commands { get; }
    public string[] Namespaces { get; }
    public string CurrentNamespace { get; }
    
    public string[]? CommandHistory { get; }
    
    
    public string RootRealCsNamespace { get; }
    public string[]? CommandRealCsNamespace { get; }
}