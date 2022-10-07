using Avalonia.Controls;

namespace GenericDashboard;

public class Dashboard
{

    private static int _idCounter = 0;
    
    
    private DashboardUc _dashboardUc = new();
    public string Name { get; set; } = $"Dashboard {_idCounter++}";

    public UserControl GetDashboard()
    {
        return _dashboardUc;
    }

    public void AddItem(UserControl item)
    {
        _dashboardUc.AddItem(item);
    }
}