namespace DroneManager.Interface.RemoteHardware;

public interface IRemoteRegister
{
    public string RegisterName { get; }
    public string? RegisterDescription { get; }
    public Type RegisterDataType { get; } //int, float, double, string, bool, byte, byte[], enum
    public object RegisterValue { get; set; }
}