using System;
using System.Windows.Controls;
using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.Remote;
using DroneManager.Interface.RemoteHardware;
using GraphicalConsole.Views.BaseViews;

namespace GraphicalConsole.Views;

public partial class DroneView : UserControl, IDisposable
{
    private Drone _drone;

    public Drone Drone
    {
        get => _drone;
        set
        {
            _drone = value;
            CcDroneIdView.Content = new DroneIdView(_drone.Id);
            CcVitalViewControl.Content = new VitalView(_drone.Vitals);
            CcLocationViewControl.Content = new LocationView(_drone.CurrentLocation);
            if (_drone.Control?.ControllableHardware != null)
                CcDroneRegisterViewControl.Content =
                    new DroneRegisterView(_drone.Control.ControllableHardware.Registers);
        }
    }

    public DroneView(Drone drone)
    {
        InitializeComponent();
        Drone = drone;
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        _drone.OnIdChanged += OnIdChanged;
        _drone.OnLocationChanged += OnLocationChanged;
        _drone.OnVitalChanged += OnVitalChanged;
        _drone.OnRemoteRegisterChanged += OnRemoteRegisterChanged;
    }

    private void OnRemoteRegisterChanged(DroneRemoteRegister[] obj)
    {
        Dispatcher.Invoke(() =>
        {
            if (CcDroneRegisterViewControl.Content == null)
            {
                CcDroneRegisterViewControl.Content = new DroneRegisterView(obj);
                return;
            }

            (CcDroneRegisterViewControl.Content as DroneRegisterView).ControllableHardware = obj;
        });
    }

    private void OnVitalChanged(VitalDto obj)
    {
        Dispatcher.Invoke(() => (CcVitalViewControl.Content as VitalView).Vital = obj);
    }

    private void OnLocationChanged(Location obj)
    {
        Dispatcher.Invoke(() => (CcLocationViewControl.Content as LocationView).Location = obj);
    }

    private void OnIdChanged(DroneId obj)
    {
        Dispatcher.Invoke(() => (CcDroneIdView.Content as DroneIdView).DroneId = obj);
    }


    public void Dispose()
    {
        _drone.OnIdChanged -= OnIdChanged;
        _drone.OnLocationChanged -= OnLocationChanged;
        _drone.OnVitalChanged -= OnVitalChanged;
        _drone.OnRemoteRegisterChanged -= OnRemoteRegisterChanged;
    }
}