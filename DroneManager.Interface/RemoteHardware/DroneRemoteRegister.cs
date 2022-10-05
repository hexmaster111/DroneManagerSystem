namespace DroneManager.Interface.RemoteHardware;

public abstract class DroneRemoteRegister
{
    public abstract string Name { get; }
    public abstract DataType DataType { get; }
    public abstract object Value { get; set; }
}