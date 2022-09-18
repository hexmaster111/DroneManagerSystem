using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.RemoteHardware;

namespace DroneManager.Interface.Remote;

public interface IControl
{
    public Queue<ITask> Tasks { get; }
    public ControlMode Mode { get; set; }
    public IControllableHardware?[]? ControllableHardware { get; }
}