using System.Windows;
using System.Windows.Controls;
using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.Remote;
using DroneManager.Interface.RemoteHardware;
using GraphicalConsole.BaseUcs;

namespace GraphicalConsole.Views;

public partial class DroneView : UserControl
{
    public Drone Drone
    {
        set
        {
            DroneId = value.Id;
            Vitals = value.Vitals;
            Location = value.CurrentLocation;
            DroneControl = value.Control?.ControllableHardware;
        }
    }

    //Dependency Property builder
    public static readonly DependencyProperty DroneIdProperty = DependencyProperty.Register(
        nameof(DroneId), typeof(DroneId), typeof(DroneView), new PropertyMetadata(default(DroneId)));

    public DroneId DroneId
    {
        get => (DroneId)GetValue(DroneIdProperty);
        set => SetValue(DroneIdProperty, value);
    }

    public static readonly DependencyProperty VitalsProperty = DependencyProperty.Register(
        nameof(Vitals), typeof(VitalDto), typeof(DroneView), new PropertyMetadata(default(VitalDto)));

    public VitalDto Vitals
    {
        get => (VitalDto)GetValue(VitalsProperty);
        set => SetValue(VitalsProperty, value);
    }

    public static readonly DependencyProperty LocationProperty = DependencyProperty.Register(
        nameof(Location), typeof(Location), typeof(DroneView), new PropertyMetadata(default(Location)));

    public Location Location
    {
        get => (Location)GetValue(LocationProperty);
        set => SetValue(LocationProperty, value);
    }


    public static readonly DependencyProperty DroneControlProperty = DependencyProperty.Register(
        nameof(DroneControl), typeof(DroneControllableHardware), typeof(DroneView),
        new PropertyMetadata(default(DroneControllableHardware)));

    public DroneControllableHardware? DroneControl
    {
        get => (DroneControllableHardware)GetValue(DroneControlProperty);
        set => SetValue(DroneControlProperty, value);
    }


    public void Initialize()
    {
        InitializeComponent();
    }
    
    public DroneView()
    {
    }
}