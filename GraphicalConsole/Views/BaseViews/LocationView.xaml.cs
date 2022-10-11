using System.Windows.Controls;
using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;

namespace GraphicalConsole.Views.BaseViews;

public partial class LocationView : UserControl
{
    public LocationView(Location location)
    {
        InitializeComponent();
        Location = location;
    }

    private Location _location;

    public Location Location
    {
        get => _location;
        set
        {
            if (value == null) return;
            _location = value;
            TbLocationName.Text = _location.LocationName ?? "No name";
            TbAddress.Text = _location.LocationAddress ?? "No address";
            TbLatitude.Text = _location.Latitude.ToString() ?? "No latitude";
            TbLongitude.Text = _location.Longitude.ToString() ?? "No longitude";
            TbLocationProvider.Text = _location.LocationProvider ?? "No provider";
        }
    }
}