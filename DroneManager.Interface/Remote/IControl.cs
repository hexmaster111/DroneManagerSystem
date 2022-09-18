using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;

namespace DroneManager.Interface.Remote;

public interface IControl
{
    public Queue<ITask> Tasks { get; }
    public IPositioning Location { get; }
    public ControlMode Mode { get; }
}