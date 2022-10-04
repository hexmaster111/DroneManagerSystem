namespace DroneManager.Interface.RemoteHardware;

public interface IRemoteRegister
{
    public string Name { get; }
    public DataType DataType { get; } //int, float, double, string, bool, byte, byte[], enum
    public object Value { get; set; }
}