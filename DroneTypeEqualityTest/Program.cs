// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using DroneManager.Interface.GenericTypes;

Console.WriteLine("Hello, World!");

var droneTypeA = new DroneId(DroneType.Experimental, 5050);
var droneTypeB = new DroneId(DroneType.Experimental, 5050);

Console.WriteLine(Equals(droneTypeA, droneTypeB));
Debugger.Break();