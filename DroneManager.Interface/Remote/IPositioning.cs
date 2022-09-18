using DroneManager.Interface.GenericTypes;

namespace DroneManager.Interface.Remote;

public interface IPositioning
{
    public Location Target { get; set; }
}