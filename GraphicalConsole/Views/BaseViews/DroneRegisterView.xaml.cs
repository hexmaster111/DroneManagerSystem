using System.Windows.Controls;
using DroneManager.Interface.RemoteHardware;

namespace GraphicalConsole.BaseUcs;

public partial class DroneRegisterView : UserControl
{
    public DroneControllableHardware ControllableHardware
    {
        set => this.DataContext = value;
    }

    public DroneRegisterView()
    {
        InitializeComponent();
    }
}