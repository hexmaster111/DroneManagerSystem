using System.Collections.Generic;
using System.Windows;
using DroneManager.Interface.GenericTypes;
using GraphicalConsole.Views;

namespace GraphicalConsole;

public class MainWindowView : DependencyObject
{
    public List<DroneView> ConnectedDrones { get; set; } = new List<DroneView>();
}