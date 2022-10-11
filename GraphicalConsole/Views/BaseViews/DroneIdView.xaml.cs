using System.Windows.Controls;
using DroneManager.Interface.GenericTypes;

namespace GraphicalConsole.Views.BaseViews;

public partial class DroneIdView : UserControl
{

    private DroneId _droneId = new DroneId(DroneType.Experimental, 0);

    public DroneId DroneId
    {
        get => _droneId;
        set
        {
            _droneId = value;
            TbDroneId.Text = _droneId.ToString();
        }
    }
    

    public DroneIdView(DroneId droneId)
    {
        InitializeComponent();
        DroneId = droneId;
    }
}