using DroneManager.Interface.GenericTypes.BaseTypes;

namespace DroneManager.Interface.GenericTypes;

public interface ITask
{
    public string Name { get; }
    public string Description { get; }
    public TaskType Type { get; }
    public DroneTaskStatus Status { get; set; }
    // Must be serializable POCO objects
    public object[] Parameters { get; set; } 
}