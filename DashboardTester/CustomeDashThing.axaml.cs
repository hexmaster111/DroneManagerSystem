using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DashboardTester;

public partial class CustomeDashThing : UserControl
{
    public CustomeDashThing()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}