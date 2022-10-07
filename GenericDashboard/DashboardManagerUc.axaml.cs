using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace GenericDashboard;

public partial class DashboardManagerUc : UserControl
{
    public DashboardManagerUc()
    {
        InitializeComponent();
    }

    public DashboardManager ParentDashboardManager;

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public void UpdateCcDashboard(List<Dashboard> dashboards)
    {
        if (ParentDashboardManager == null)
            throw new Exception("Can not update dashboard, parent dashboard manager is null");
        //Fill in the MiGotoDash MenuItems
        var gotoDashMenu = this.FindControl<MenuItem>("MiGotoDash");
        gotoDashMenu.Items = null;

        var newItems = new List<MenuItem>();

        foreach (var dash in dashboards)
        {
            var mi = new MenuItem();
            mi.Header = dash.Name;
            mi.Click += (sender, args) => { ParentDashboardManager.LoadDashboard(dash); };
            newItems.Add(mi);
        }

        gotoDashMenu.Items = newItems;
    }

    private void DashboardAdd_Click(object? sender, RoutedEventArgs e)
    {
        ParentDashboardManager.AddDashboard();
    }

    public void SetActiveDash(Dashboard dash)
    {
        this.FindControl<ContentControl>("CcDashboard").Content = dash;
    }

    private void DEBUGAddTestControl_onClick(object? sender, RoutedEventArgs e)
    {
        ParentDashboardManager.AddTestControl();
    }

    public void AddTestControl()
    {
        this.Find
    }
}