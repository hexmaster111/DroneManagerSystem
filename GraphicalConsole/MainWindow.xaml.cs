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
            ServerBackendAbstraction.RemoteClientManagerFacade.OnDroneConnected += RemoteClientManagerFacade_OnDroneConnected;
            
        }

        private void RemoteClientManagerFacade_OnDroneConnected(DroneId obj)
        {
            throw new System.NotImplementedException();
        }
    }
}