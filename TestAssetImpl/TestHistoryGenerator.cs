using DroneManager.Interface.History;

namespace TestAssetImpl;

public static class TestHistoryGenerator
{
    private static int _CountToGenerate = 10;

    public static TaskHistory[] Generate()
    {
        var history = new TaskHistory[_CountToGenerate];
        for (var i = 0; i < _CountToGenerate; i++)
        {
            history[i] = new TaskHistory
            {
                Task = new TestTaskGenerator(i),
                Notes = "Test Notes",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now + TimeSpan.FromHours(5),
            };
        }

        return history;
    }
}