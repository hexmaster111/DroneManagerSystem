namespace DroneManager.Interface.RemoteHardware;

public abstract class DroneControllableHardware
{
    public abstract ControllableHardwareMetaData GetHardwareMetaData();
    public abstract DroneRemoteRegister[] Registers { get; init; }
}