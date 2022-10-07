using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace GenericDashboard;

public partial class DashboardUc : UserControl
{
    public DashboardUc()
    {
        InitializeComponent();
    }

    public void AddItem(UserControl item)
    {
        var grid = this.FindControl<Grid>("Grid");
        var holder = DashItemWrapper.Create(item);
        grid.Children.Add(holder);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
}