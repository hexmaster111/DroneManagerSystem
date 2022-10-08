using System;
using System.ComponentModel;
using System.Windows;
using DroneManager.Interface.GenericTypes;
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
            a.ConnectedDrones = ServerBackendAbstraction.RemoteClientManagerFacade.GetDrones();
            Dispatcher.Invoke(() => { this.DataContext = a; });
            //TODO: Time to refresh to toplevel uis things
        }
    }
}