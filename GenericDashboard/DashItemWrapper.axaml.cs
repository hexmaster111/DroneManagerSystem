using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GenericDashboard;

public partial class DashItemWrapper : UserControl
{
    public DashItemWrapper()
    {
        InitializeComponent();
    }

    public static DashItemWrapper Create(UserControl item)
    {
        return new DashItemWrapper
        {
            Content = item
        };
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}