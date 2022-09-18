using DroneManager.DocsHelper;

namespace DroneManager.Interface.RemoteHardware;

public class ControllableHardwareMetaData
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public Guid SerialNumber { get; set; }
    public string FirmwareVersion { get; set; }
    public string HardwareVersion { get; set; }
    public string SoftwareVersion { get; set; }
    public Document Documentation { get; set; }
}