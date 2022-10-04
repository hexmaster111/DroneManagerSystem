using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.RemoteHardware;

namespace DroneManager.Interface.Remote;

public abstract class DroneControl
{
    public abstract ITask Task { get; }
    public abstract DroneControllableHardware[]? ControllableHardware { get; }
}