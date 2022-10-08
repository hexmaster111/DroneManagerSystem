using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.Remote;
using DroneManager.Interface.RemoteConnection;

namespace DroneManager.Interface.GenericTypes;

public class Drone
{
    public Location CurrentLocation { get; }
    public VitalDto Vitals { get; }
    public DroneControl? Control { get; }
    public DroneId Id { get; init; }
}