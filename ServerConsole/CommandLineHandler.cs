using System.Reflection;
using IConsoleLogInterface;
using ServerConsole.Commands;

namespace ServerConsole;

public class CommandLineHandler : ICommandManager
{
    private readonly bool _running = true;
    private IConsoleLog _log;

    private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
    {
        return assembly.GetTypes().Where(type => type.Namespace.Contains(nameSpace)).ToArray();
    }


    private static readonly string commandNamespace = "ServerConsole.Commands.RootNamespace";

    private ICommand?[] GetCommands()
    {
        var commands = new List<ICommand>();
        var commandTypes = GetTypesInNamespace(Assembly.GetExecutingAssembly(), commandNamespace);
        foreach (var commandType in commandTypes)
        {
            ICommand? command = null;

            try
            {
                command = (ICommand?)Activator.CreateInstance(commandType);
            }
            catch (InvalidCastException e)
            {
                _log.WriteLog(
                    message: $"Unable to load command {commandType.Name} because it does not implement ICommand",
                    logLevel: LogLevel.Error);
            }

            if (command == null) continue;
            //Remove the root namespace from the commands namespace
            command.RuntimeAssignedNamespace = commandType.Namespace.Remove(0, commandNamespace.Length);
            if (command.RuntimeAssignedNamespace.Length > 0)
                command.RuntimeAssignedNamespace = command.RuntimeAssignedNamespace.Remove(0, 1);

            command.CommandManager = this;
            commands.Add(command);
        }

        return commands.ToArray();
    }

    public string CurrentNamespace { get; private set; } = "";

    private ICommand[] _commands;

    public string[] AvailableNamespaces =>
        _commands.Select(command => command.RuntimeAssignedNamespace).Distinct().ToArray();

    public CommandLineHandler(IConsoleLog log)
    {
        _log = log;
        _commands = GetCommands();
    }


    public void StartReadThread()
    {
        var readThread = new Thread(ConsoleReadLoopNonReturning);
        readThread.Start();
    }

    private void ConsoleReadLoopNonReturning()
    {
        while (_running)
        {
            var readLine = Console.ReadLine();
            if (readLine == null) continue;
            var command = readLine.Split(' ');
            if (command.Length == 0) continue;
            _handleNewCommand(command);
        }
    }

    private void _handleCommandOutput(CommandResults results)
    {
        //Check if the new namespace is valid
        if (results.NewNamespace != null)
            if (_commands.Any(c => c.RuntimeAssignedNamespace == results.NewNamespace))
                CurrentNamespace = results.NewNamespace;
            else
                _log.WriteCommandLog(results.CommandRan
                    , $"Unable to change namespace to {results.NewNamespace} because it does not exist",
                    LogLevel.Error);

        if (results.Error != null)
            _log.WriteCommandLog(results.CommandRan, results.Error, LogLevel.Error);

        if (results.Output != null)
            _log.WriteCommandLog(results.CommandRan, $"Command output:\r\n" + results.Output, LogLevel.Info);
    }

    private CommandResults _handleExecution(ICommand commandToRun, string?[] args)
    {
        commandToRun.Execute(args, out var output, out var errorOut, out var newNamespace);
        return new CommandResults(output, errorOut, newNamespace, args[0] ?? "null cmd");
    }

    private class CommandResults
    {
        public CommandResults(string? output, string? error, string? newNamespace, string commandRan)
        {
            Output = output;
            Error = error;
            NewNamespace = newNamespace;
            CommandRan = commandRan;
        }

        public string CommandRan { get; }

        public string? Output { get; set; }
        public string? Error { get; set; }
        public string? NewNamespace { get; set; }
    }


    private bool __checkForCommand(ICommand commandBeingChecked, string commandWanted)
    {
        //the commandBeingChecked is the command that is wanted
        if (commandBeingChecked.Name == commandWanted ||
            (commandBeingChecked.Aliases ?? Array.Empty<string>()).Contains(commandWanted))
        {
            if (commandBeingChecked.RuntimeAssignedNamespace == CurrentNamespace)
                return true;
            if (commandBeingChecked.RuntimeAssignedNamespace == "")
                return true;
        }

        return false;
    }

    private void _handleNewCommand(string[] command)
    {
        foreach (var cmd in _commands)
        {
            if (!__checkForCommand(cmd, command[0])) continue;
            _handleCommandOutput(_handleExecution(cmd, command));
            return;
        }

        _log.WriteLog(message: $"Unable to find command {command[0]}", logLevel: LogLevel.Error);
    }

    public ICommand[] Commands => _commands;

    public string[] Namespaces
    {
        get
        {
            var namespaces = new List<string>();
            foreach (var command in _commands)
            {
                if (namespaces.Contains(command.RuntimeAssignedNamespace)) continue;
                namespaces.Add(command.RuntimeAssignedNamespace);
            }

            return AvailableNamespaces;
        }
    }
}