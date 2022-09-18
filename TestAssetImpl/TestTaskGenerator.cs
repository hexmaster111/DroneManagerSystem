using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;

namespace TestAssetImpl;

public class TestTaskGenerator : ITask
{
    public TestTaskGenerator(int i)
    {
        Name = $"Task {i}";
        Description = $"Description {i}";
        Status = DroneTaskStatus.Completed;
        Type = TaskType.Debug;
        Parameters = new string[] { "Param0", "Param1" }; 
    }
    
    public string Name { get; }
    public string Description { get; }
    public TaskType Type { get; }
    public DroneTaskStatus Status { get; set; }
    public object[] Parameters { get; set; }
}