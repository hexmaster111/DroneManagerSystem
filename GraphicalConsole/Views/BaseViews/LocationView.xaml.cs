using System.Windows.Controls;
using DroneManager.Interface.GenericTypes.BaseTypes;

namespace GraphicalConsole.BaseUcs;

public partial class LocationView : UserControl
{
    public LocationView()
    {
        InitializeComponent();
    }


    public Location Location
    {
        set => this.DataContext = value;
    }
}