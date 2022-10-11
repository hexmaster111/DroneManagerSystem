using System.Windows.Controls;
using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.Remote;

namespace GraphicalConsole.Views.BaseViews;

public partial class VitalView : UserControl
{
    private VitalDto _vital;

    public VitalDto Vital
    {
        get => _vital;
        set
        {
            _vital = value;
            if (value == null) return;
            TbTemperature.Text = value.Temperature.ToString();
            TbBreathingRate.Text = value.BreathingRate.ToString();
            TbHeartRate.Text = value.HeartRate.ToString();
        }
    }


    public VitalView(VitalDto vital)
    {
        InitializeComponent();
        Vital = vital;
    }
}