using ConsoleCommandHandler.Commands;

namespace ConsoleCommandHandler;

public interface ICommandAdder
{
    public void AddCommand(ICommand command);
    public void RemoveCommand(ICommand command);
}