namespace DroneManager.Interface.RemoteHardware;

public interface IControllableHardware
{
     public ControllableHardwareMetaData GetHardwareMetaData();
     public IRemoteRegister[] Registers { get; }
     
}