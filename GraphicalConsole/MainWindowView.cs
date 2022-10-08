using System.Collections.Generic;
using DroneManager.Interface.GenericTypes;
using GraphicalConsole.Views;

namespace GraphicalConsole;

public class MainWindowView
{
    public class ConnectedDroneView
    {
        public Drone ViewingDrone { get; set; }
        public DroneView DroneView { get; set; }
    }
    
    public List<ConnectedDroneView> ConnectedDrones { get; set; }
}