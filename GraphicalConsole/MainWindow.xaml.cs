using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using DroneManager.Interface.GenericTypes;
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

            WpfConsoleHelper.ShowConsole();
            ServerBackendAbstraction.StartServer();
            
            // ServerBackendAbstraction.RemoteClientManagerFacade.OnDroneConnected +=
            //     RemoteClientManagerFacade_OnDroneConnected;
            
            this.Closing += MainWindow_Closing;
            DroneDashView.Content = new DroneDashView();
        }

        private void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            //TODO: Hide the window and let the cli be able to open it again
            ServerBackendAbstraction.StopServer();
        }

    }
}