namespace DroneManager.Interface.RemoteHardware;

public abstract class DroneControllableHardware
{
    public abstract ControllableHardwareMetaData GetHardwareMetaData();
    public DroneRemoteRegister[] Registers { get; }
}