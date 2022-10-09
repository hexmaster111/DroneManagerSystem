using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using DroneManager.Interface.GenericTypes;
using GraphicalConsole.BaseUcs;
using GraphicalConsole.Views;
using HaileysHelpers;

namespace GraphicalConsole
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow _instance;

        public MainWindow()
        {
            InitializeComponent();

            WpfConsoleHelper.ShowConsole();
            ServerBackendAbstraction.StartServer();
            ServerBackendAbstraction.RemoteClientManagerFacade.OnDroneConnected +=
                RemoteClientManagerFacade_OnDroneConnected;
            this.Closing += MainWindow_Closing;
            DroneDashView.Content = new DroneDashView();
            _instance = this;
        }

        private void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            //TODO: Hide the window and let the cli be able to open it again
            ServerBackendAbstraction.StopServer();
            // this.Hide();
            // e.Cancel = true;
        }


        private void RemoteClientManagerFacade_OnDroneConnected(DroneId obj)
        {
            var a = new MainWindowView();
            var drones = ServerBackendAbstraction.RemoteClientManagerFacade.GetDrones();
            Dispatcher.Invoke(() => { (DroneDashView.Content as DroneDashView).Drones = drones.ToList(); });
            //
            // a.ConnectedDrones = droneViewModelList;
            //
            // Dispatcher.Invoke(() => { this.DataContext = a; });
            //TODO: Time to refresh to toplevel uis things
        }
    }
}