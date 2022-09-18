using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;

namespace DroneManager.Interface.Remote;

public interface IPositioning
{
    public Location Target { get; set; }
}