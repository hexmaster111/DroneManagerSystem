using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.Remote;
using DroneManager.Interface.RemoteConnection;

namespace DroneManager.Interface.GenericTypes;

public interface IDrone
{
    public Location CurrentLocation { get; }
    public IVital Vitals { get; }
    public DroneControl Control { get; }
    public DroneId Id { get; }
}