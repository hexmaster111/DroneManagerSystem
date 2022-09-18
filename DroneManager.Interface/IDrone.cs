
using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.Remote;
using DroneManager.Interface.RemoteConnection;

namespace DroneManager.Interface;

public interface IDrone
{
    public IRemoteConnection Connection { get; }
    public Location CurrentLocation { get; }
    public IVital Vitals { get; }
    public IControl Control { get; }
}