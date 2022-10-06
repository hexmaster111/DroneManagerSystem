using DroneManager.DocsHelper;

namespace DroneManager.Interface.RemoteHardware;

public class ControllableHardwareMetaData
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Document? Documentation { get; set; }
}