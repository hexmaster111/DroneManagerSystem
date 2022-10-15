using System.Collections.Generic;
using System.Windows;
using DroneManager.Interface.GenericTypes;
using GraphicalConsole.Views;
using GraphicalConsole.Views.BaseViews;

namespace GraphicalConsole;

public class MainWindowView : DependencyObject
{
    public List<DroneView> ConnectedDrones { get; set; } = new List<DroneView>();
}