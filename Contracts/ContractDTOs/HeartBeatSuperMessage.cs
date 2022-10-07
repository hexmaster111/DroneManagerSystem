using DroneManager.Interface.RemoteHardware;
using GenericMessaging;

namespace Contracts.ContractDTOs;

public class HeartBeatSuperMessage : SenableDtoBase 
{
    public HeartBeatSuperMessage(LocationMessage location, HardwareInfoUpdateMessage hardwareInfo, VitalsUpdateMessage vitals)
    {
        Location = location;
        HardwareInfo = hardwareInfo;
        Vitals = vitals;
    }

    public LocationMessage Location { get; set; }
    public HardwareInfoUpdateMessage HardwareInfo { get; set; }
    public VitalsUpdateMessage Vitals { get; set; }
}