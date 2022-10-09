using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using DroneManager.Interface.GenericTypes;

namespace GraphicalConsole.Views;

public partial class DroneDashView : UserControl, INotifyPropertyChanged
{
    public static readonly DependencyProperty DronesProperty = DependencyProperty.Register(
        nameof(Drones), typeof(List<Drone>), typeof(MainWindowView), new PropertyMetadata(null));


    public List<Drone> Drones
    {
        get => (List<Drone>)GetValue(DronesProperty);
        set
        {
            SetValue(DronesProperty, value);
            OnPropertyChanged();
        }
    }


    //Dp for the selected drone
    public static readonly DependencyProperty SelectedDroneProperty = DependencyProperty.Register(
        nameof(SelectedDrone), typeof(Drone), typeof(MainWindowView), new PropertyMetadata(null));

    public Drone SelectedDrone
    {
        get => (Drone)GetValue(SelectedDroneProperty);
        set
        {
            SetValue(SelectedDroneProperty, value);
            OnPropertyChanged();
            CcSelectedDrone.Content = new DroneView(value);
        }
    }


    public DroneDashView()
    {
        InitializeComponent();
        DataContext = this;
    }

    private void OnDroneSet(object sender, RoutedEventArgs e)
    {
        SelectedDrone = (Drone)((RadioButton)sender).DataContext;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}