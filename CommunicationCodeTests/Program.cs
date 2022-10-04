using System.Diagnostics;
using DroneManager.Interface.DroneCommunicationCodes;

var Codes = CommunicationCode.CommunicationCodes;

Console.WriteLine("Hello, World!");


foreach (var code in Codes)
{
    Console.WriteLine(code.ToString());
}

Debugger.Break();