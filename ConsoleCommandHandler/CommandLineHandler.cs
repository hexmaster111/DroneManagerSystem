using System.Reflection;
using ConsoleCommandHandler.Commands;
using IConsoleLog;
using ICommand = ConsoleCommandHandler.Commands.ICommand;

namespace ConsoleCommandHandler;

public class CommandLineHandler : ICommandManager
{
    private bool _running = true;
    private IConsoleLog.IConsoleLog _log;

    private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
    {
        return assembly.GetTypes().Where(type => type.Namespace.Contains(nameSpace)).ToArray();
    }

    private ICommand?[] GetCommands(string fromNamespace, Assembly assembly)
    {
        var commands = new List<ICommand>();
        var commandTypes = GetTypesInNamespace(assembly, fromNamespace);
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
            command.RuntimeAssignedNamespace = commandType.Namespace.Remove(0, fromNamespace.Length);
            if (command.RuntimeAssignedNamespace.Length > 0)
                command.RuntimeAssignedNamespace = command.RuntimeAssignedNamespace.Remove(0, 1);

            command.CommandManager = this;
            commands.Add(command);
        }

        return commands.ToArray();
    }

    public string CurrentNamespace { get; private set; } = "";
    public string[]? CommandHistory => _commandHistory.ToArray();
    public string RootRealCsNamespace { get; }
    public string[]? CommandRealCsNamespace { get; }

    private ICommand[] _commands;

    public string[] AvailableCommandNamespaces =>
        _commands.Select(command => command.RuntimeAssignedNamespace).Distinct().ToArray();

    public void Start()
    {
        _running = true;
        StartReadThread();
    }

    public void Stop()
    {
        _running = false;
    }

    public CommandLineHandler(IConsoleLog.IConsoleLog log, string rootNamespace, string[]? commandNamespace, Assembly commandAssembly)
    {
        _log = log;
        RootRealCsNamespace = rootNamespace;
        CommandRealCsNamespace = commandNamespace;

        List<ICommand> allCommands =
            (GetCommands(RootRealCsNamespace, Assembly.GetExecutingAssembly()).ToList() ?? throw new InvalidOperationException())!;

        if (CommandRealCsNamespace != null)
        {
            foreach (var commandNamespace1 in CommandRealCsNamespace)
            {
                allCommands.AddRange(GetCommands(commandNamespace1, commandAssembly)!);
            }
        }

        _commands = allCommands.ToArray();
    }


    public void StartReadThread()
    {
        var readThread = new Thread(ConsoleReadLoopNonReturning);
        readThread.Start();
    }

    //from https://stackoverflow.com/a/8946847
    public static void ClearCurrentConsoleLine()
    {
        var currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLineCursor);
    }

    private List<string> _commandHistory = new();

    private void ConsoleReadLoopNonReturning()
    {
        List<char> currentUserInput = new();
        List<char> beforeHistoryBuffer = new();

        int commandHistoryIndex = 0;

        while (_running)
        {
            //new way with keyboard shortcuts
            ClearCurrentConsoleLine();
            Console.Write($"{CurrentNamespace}> {currentUserInput.Aggregate("", (current, c) => current + c)}");
            var input = Console.ReadKey();
            switch (input.Key)
            {
                case ConsoleKey.Tab:
                    //try to get the command
                    var compleateCommandString = new string(currentUserInput.ToArray());
                    //Split the command into arguments
                    var autoCompleatWholeCommmand = compleateCommandString.Split(' ');

                    //if the first entry is empty, continue
                    if (autoCompleatWholeCommmand[0] == "") continue;

                    //get the command
                    var autoCompleteCommand = _getCommand(autoCompleatWholeCommmand[0]);

                    //if the command is null, continue
                    if (autoCompleteCommand == null) continue;

                    _handleAutoComplete(autoCompleteCommand, autoCompleatWholeCommmand, out var autoCompleatResult);

                    //Remove the old last argument
                    for (var i = 0; i < autoCompleatWholeCommmand[^1].Length; i++)
                    {
                        currentUserInput.RemoveAt(currentUserInput.Count - 1);
                    }

                    //add the result to the current user input
                    currentUserInput.AddRange(autoCompleatResult);

                    continue;
                //Do command and continue
                case ConsoleKey.Enter: break; //Just brake, the command is already in current user input.

                case ConsoleKey.DownArrow:
                case ConsoleKey.UpArrow:
                    // Console.Write("\b");

                    switch (input.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (commandHistoryIndex == 0) // if the user was at the end of the history
                                beforeHistoryBuffer = currentUserInput; // save the current input
                            commandHistoryIndex++;
                            break;
                        case ConsoleKey.DownArrow:
                            commandHistoryIndex--;
                            break;
                    }

                    if (commandHistoryIndex == 0) // The user is at the end of the history
                    {
                        currentUserInput = beforeHistoryBuffer;
                        continue;
                    }

                    if (commandHistoryIndex > _commandHistory.Count) // The user is at the top of the history
                    {
                        commandHistoryIndex = _commandHistory.Count;
                        continue;
                    }

                    if (commandHistoryIndex < 0) //The user pushed down arrow too much
                    {
                        commandHistoryIndex = 0;
                        continue;
                    }

                    //The index is flipped because the newest command is at the end of the list
                    var flippedIndex = _commandHistory.Count - commandHistoryIndex;

                    //Set what the console is showing to the command in the history
                    currentUserInput = _commandHistory[flippedIndex].ToList();


                    continue;

                case ConsoleKey.RightArrow: //TODO: Implement cursor movement
                case ConsoleKey.LeftArrow:
                    Console.Write("\b");
                    continue;

                case ConsoleKey.Backspace:
                    if (currentUserInput.Count == 0) continue;
                    currentUserInput.RemoveAt(currentUserInput.Count - 1);
                    Console.Write(" \b");
                    continue;

                default:
                    currentUserInput.Add(input.KeyChar);
                    continue;
            }

            var command = currentUserInput.ToArray();
            currentUserInput.Clear();
            commandHistoryIndex = 0;

            //Turn the command into a string
            var commandString = new string(command);
            //Split the command into arguments
            var commandArgs = commandString.Split(' ');

            //if the first entry is empty, continue
            if (commandArgs[0] == "") continue;

            //if the command was executed, save it to history and continue
            _commandHistory.Add(commandString);
            if (_handleNewCommand(commandArgs))
            {
            }
            else
                _log.WriteLog(message: "Command Not Found", logLevel: LogLevel.Error);
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

    private void _handleAutoComplete(ICommand? commandEntered, string[]? currentEnteredArguments,
        out string autoComplete)
    {
        autoComplete = "";

        //if the command or its arguments are null, return
        if (commandEntered?.Arguments == null) return;
        //get the last argument, this will be what we are trying to compete
        var lastArgument = currentEnteredArguments?[^1];

        //if the last argument is null, continue
        if (lastArgument == null) return;

        //Get the index of the current last argument (-2 because the first argument is the command)
        var lastArgumentIndex = currentEnteredArguments?.Length - 2;

        //if the last argument index is below 0, continue
        if (lastArgumentIndex < 0) return;

        //get the auto complete options
        var autoCompleteOptions = commandEntered.Arguments[(Index)lastArgumentIndex].HelperType;

        switch (autoCompleteOptions)
        {
            case Argument.CompleteHelperType.None: break; //Do nothing, this would be a name or something
            case Argument.CompleteHelperType.Namespace: //Get all the namespaces that start with the last argument

                //Get all the namespaces that start with the last argument
                var autoCompleteNamespaces = _commands
                    .Where(c => c.RuntimeAssignedNamespace.StartsWith(lastArgument))
                    .Select(c => c.RuntimeAssignedNamespace)
                    .ToList();

                //Add all the namespaces that are within the currentNamespace
                autoCompleteNamespaces.AddRange(_commands
                    .Where(c => c.RuntimeAssignedNamespace.StartsWith(CurrentNamespace))
                    .Select(c => c.RuntimeAssignedNamespace)
                    .ToList());

                //if there are no namespaces, continue
                if (autoCompleteNamespaces.Count == 0) return;

                autoComplete = autoCompleteNamespaces[0];

                break;
            case Argument.CompleteHelperType.TrueFalse: //figure out what the user has started typing and fill that in
                if (lastArgument.ToLower().Contains('t'))
                    autoComplete = "rue";
                else if (lastArgument.ToLower().Contains('f'))
                    autoComplete = "alse";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private ICommand? _getCommand(string commandName)
    {
        foreach (var command in _commands)
        {
            if ((command.Aliases == null || !command.Aliases.Contains(commandName)) &&
                command.Name != commandName) continue;

            //Check if the command is in the current namespace or the global namespace
            if (command.RuntimeAssignedNamespace == CurrentNamespace || command.RuntimeAssignedNamespace == "")
                return command;
        }

        return null;
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

    private bool _handleNewCommand(string[] command)
    {
        foreach (var cmd in _commands)
        {
            if (!__checkForCommand(cmd, command[0])) continue;
            _handleCommandOutput(_handleExecution(cmd, command));
            return true;
        }

        return false;
    }

    public void ExecuteCommand(string command)
    {
        var commandArgs = command.Split(' ');
        _handleNewCommand(commandArgs);
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

            return AvailableCommandNamespaces;
        }
    }
}