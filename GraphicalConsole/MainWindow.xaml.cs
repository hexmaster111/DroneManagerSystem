using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
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
        public MainWindow()
        {
            InitializeComponent();

            // var id = new DroneIdView(new DroneId(DroneType.Experimental, 1));
            var a = new DroneView();

            a.Drone = new Drone() { Id = new DroneId(DroneType.Experimental, 1) };

            
            a.Initialize();
            CcTest.Content = a;

            return;
            WpfConsoleHelper.ShowConsole();
            ServerBackendAbstraction.StartServer();
            ServerBackendAbstraction.RemoteClientManagerFacade.OnDroneConnected +=
                RemoteClientManagerFacade_OnDroneConnected;
            this.Closing += MainWindow_Closing;
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

            var droneViewModel = new List<MainWindowView.ConnectedDroneView>();
            foreach (var drone in drones)
            {
                Dispatcher.Invoke(() =>
                {
                    droneViewModel.Add(new MainWindowView.ConnectedDroneView()
                    {
                        DroneView = new DroneView(),
                        ViewingDrone = new Drone()
                    });
                });
            }

            a.ConnectedDrones = droneViewModel;

            Dispatcher.Invoke(() => { this.DataContext = a; });
            //TODO: Time to refresh to toplevel uis things
        }
    }
}