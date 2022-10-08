using System.Windows.Controls;
using DroneManager.Interface.Remote;

namespace GraphicalConsole.BaseUcs;

public partial class VitalView : UserControl
{
    public VitalDto Vital
    {
        set
        {
            this.DataContext = value;
        }
    }


    public VitalView()
    {
        InitializeComponent();
    }
}