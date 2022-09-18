using DroneManager.Interface.DroneMetaData;
using DroneManager.Interface.GenericTypes;

namespace DroneManager.Interface;

public interface IAsset
{
    public IDrone Drone { get; }
    public Metadata Metadata { get; }
}