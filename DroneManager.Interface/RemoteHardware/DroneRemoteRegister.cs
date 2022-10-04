namespace DroneManager.Interface.RemoteHardware;

public abstract class DroneRemoteRegister
{
    public abstract string Name { get; }
    public abstract DataType DataType { get; } //int, float, double, string, bool, byte, byte[], enum
    public abstract object Value { get; set; }
}