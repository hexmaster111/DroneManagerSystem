using DroneManager.Interface.GenericTypes;

namespace DroneManager.Interface.History;

public class TaskHistory
{
    public ITask Task { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Notes { get; set; }
}