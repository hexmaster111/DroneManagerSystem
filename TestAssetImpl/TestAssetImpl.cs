using DroneManager.Interface;
using DroneManager.Interface.DroneMetaData;
using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.History;

namespace TestAssetImpl;

public class TestAssetImpl
{
    public Drone Drone => TestDroneGenerator.Generate();
    public TaskHistory[] TaskHistory { get; } = TestHistoryGenerator.Generate();
    public Metadata Metadata => TestMetadataGenerator.Generate();
}