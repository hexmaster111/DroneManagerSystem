using DroneManager.Interface.DroneMetaData;
using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.History;

namespace DroneManager.Interface;

public interface IAsset
{
    public IDrone Drone { get; }
    public TaskHistory[] TaskHistory { get; }
    public Metadata Metadata { get; }
}
