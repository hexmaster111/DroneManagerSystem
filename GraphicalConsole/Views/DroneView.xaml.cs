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
        get { return (Drone)GetValue(DroneProperty); }

        set => SetValue(DroneProperty, value);
    }


    public static readonly DependencyProperty DroneProperty =
        DependencyProperty.Register(nameof(Drone), typeof(Drone), typeof(DroneView), new PropertyMetadata(null));

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

    public static readonly DependencyProperty CurrentLocationProperty = DependencyProperty.Register(
        nameof(CurrentLocation), typeof(Location), typeof(DroneView), new PropertyMetadata(default(Location)));

    public Location CurrentLocation
    {
        get => (Location)GetValue(CurrentLocationProperty);
        set => SetValue(CurrentLocationProperty, value);
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

    public DroneView(Drone drone)
    {
        Drone = drone;
        DroneId = drone.Id;
        Vitals = drone.Vitals;
        CurrentLocation = drone.CurrentLocation;
        // DroneControl = drone.Control;
        Initialize();
    }
}