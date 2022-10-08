using System.Windows;
using System.Windows.Controls;
using DroneManager.Interface.GenericTypes;

namespace GraphicalConsole.BaseUcs;

public partial class DroneIdView : UserControl
{
    //DependencyProperty builder
    public static readonly DependencyProperty DroneIdProperty = DependencyProperty.Register(
        nameof(DroneId), typeof(DroneId), typeof(DroneIdView), new PropertyMetadata(default(DroneId)));

    public DroneId DroneId
    {
        get => (DroneId)GetValue(DroneIdProperty);
        set => SetValue(DroneIdProperty, value);
    }

    public void Initialize()
    {
        InitializeComponent();
    }


    public DroneIdView()
    {
    }
}