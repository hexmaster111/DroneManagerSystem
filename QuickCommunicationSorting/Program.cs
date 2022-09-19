using DroneManager.Interface.DroneCommunicationCodes;

bool running = true;
var communicationCodes = CommunicationCode.GetCommunicationCodes();
string command = "";
List<CommunicationCode> DisplayedCodes = new();

while (running)
{
    //Get the next command from the user
    Console.BackgroundColor = ConsoleColor.White;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.Write($">{command}");
    var commandKey = Console.ReadKey();
    Console.Clear();

    if (commandKey.Key == ConsoleKey.Backspace && command.Length > 0)
        command = command.Remove(command.Length - 1);
    else if (commandKey.Key == ConsoleKey.Backspace && command.Length == 0)
        continue;
    else
        command += commandKey.KeyChar;


    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.White;

    List<string> output = new();
    //Look through the list of communication codes to see if the command matches any of them
    foreach (var code in communicationCodes.Reverse())
    {
        DisplayedCodes.Add(code);
        if (code.CodeValue.ToLower().Contains(command.ToLower()))
        {
            Console.WriteLine(code.ToString());
            continue;
        }

        if (code.CodeId.ToString().ToLower().Contains(command))
        {
            Console.WriteLine(code.ToString());
            continue;
        }
        if (code.Type.ToString().ToLower().Contains(command))
        {
            Console.WriteLine(code.ToString());
            continue;
        }
    }

    if (commandKey.Key == ConsoleKey.Enter)
    {
        Console.Clear();
        command = "";
    }

    if (command == "quit") running = false;
}