using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.Remote;
using DroneManager.Interface.RemoteHardware;

namespace DroneManager.Interface;

public class Drone
{
    public Action<Location> OnLocationChanged { get; set; }
    public Location CurrentLocation { get; protected set; }
    public Action<VitalDto> OnVitalChanged { get; set; }
    public VitalDto Vitals { get; protected set; } = new VitalDto();
    public Action<DroneRemoteRegister[]> OnRemoteRegisterChanged { get; set; }
    public IDroneControl? Control { get; protected set; }
    public Action<DroneId> OnIdChanged { get; set; }
    public DroneId Id { get; protected set; }

    public override string ToString()
    {
        return $"Drone {Id} SuperClass";
    }
}