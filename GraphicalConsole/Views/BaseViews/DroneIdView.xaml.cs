using System.Windows.Controls;
using DroneManager.Interface.GenericTypes;

namespace GraphicalConsole.BaseUcs;

public partial class DroneIdView : UserControl
{

    public DroneId DroneId
    {
        set => this.DataContext = value;
    }

    public DroneIdView()
    {
        InitializeComponent();
    }
}