using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using DroneManager.Interface.GenericTypes;

namespace GraphicalConsole.Views;

public partial class DroneDashView : UserControl
{
    public DroneDashView()
    {
        InitializeComponent();
        ServerBackendAbstraction.RemoteClientManagerFacade.OnDroneConnected += OnDroneConnected;
    }

    private void OnDroneConnected(DroneId obj)
    {
        Dispatcher.Invoke(() =>
        {
            MiConnectedDrones.ItemsSource = ServerBackendAbstraction.RemoteClientManagerFacade.GetDrones();
        });
    }


    private void OnDroneSet(object sender, RoutedEventArgs e)
    {
        //Get the cached version of the drone

        var droneToChangeTo = (Drone)((RadioButton)sender).DataContext;

        if (ServerBackendAbstraction.RemoteClientManagerFacade.GetDrone(droneToChangeTo.Id, out var cashedDrone))
        {
            CcSelectedDrone.Content = new DroneView(cashedDrone);
            // CcSelectedDrone.Content = new DroneView((Drone)((RadioButton)sender).DataContext);
        }
    }
}