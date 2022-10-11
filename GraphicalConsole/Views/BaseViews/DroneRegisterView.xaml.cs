using System.Windows.Controls;
using DroneManager.Interface.RemoteHardware;

namespace GraphicalConsole.Views.BaseViews;

public partial class DroneRegisterView : UserControl
{
    private DroneRemoteRegister[] _controllableHardware;

    public DroneRemoteRegister[]  ControllableHardware
    {
        get => _controllableHardware;
        set => LvRegisters.ItemsSource = value;
    }

    public DroneRegisterView(DroneRemoteRegister[] controllableHardware)
    {
        InitializeComponent();
        ControllableHardware = controllableHardware;
    }
}