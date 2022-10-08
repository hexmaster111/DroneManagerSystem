using System;
using DroneManager.Interface.GenericTypes;

namespace GraphicalConsole;

public interface IRemoteClientManagerFacade
{
    public Drone[] GetDrones();
    public bool GetDrone(DroneId droneId , out Drone? drone);
    public Action<DroneId> OnDroneConnected { get; set; }
}