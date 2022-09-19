// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;
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

    command += commandKey.KeyChar;

    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.White;

    //Look through the list of communication codes to see if the command matches any of them
    foreach (var code in communicationCodes)
    {
        DisplayedCodes.Add(code);
        if (code.CodeValue.ToLower().Contains(command.ToLower())) Console.WriteLine(code.ToString());
        if (code.CodeId.ToString().ToLower().Contains(command)) Console.WriteLine(code.ToString());
    }


    if (commandKey.Key == ConsoleKey.Enter)
    {
        Console.Clear();
        command = "";
    }

    //Check if the user wants to quit
    if (command == "quit") running = false;
}