using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.RemoteHardware;

namespace DroneManager.Interface.Remote;


public interface IDroneControl
{
    public abstract DroneControllableHardware? ControllableHardware { get; }
}

public abstract class DroneControl
{
    public abstract DroneControllableHardware? ControllableHardware { get; }
}