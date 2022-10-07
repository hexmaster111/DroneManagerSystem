using Avalonia.Controls;

namespace GenericDashboard;

public class DashboardManager
{
    private DashboardManagerUc _dashboardManagerUc;

    private List<Dashboard> _dashboards = new();
    
    public DashboardManager()
    {
        _dashboardManagerUc = new DashboardManagerUc()
        {
            ParentDashboardManager = this
        };
    }

    public DashboardManagerUc GetDashboardManager => _dashboardManagerUc;

    public void AddDashboard()
    {
        _dashboards.Add(new Dashboard());
        //Update the CcDashboard in the DashboardManagerUc
        _dashboardManagerUc.UpdateCcDashboard(_dashboards);
    }


    public void LoadDashboard(Dashboard dash)
    {
        //Load the dashboard
        _dashboardManagerUc.SetActiveDash(dash.GetDashboard());
    }

    public void AddTestControl()
    {
        _dashboardManagerUc.AddTestControl();
    }
}