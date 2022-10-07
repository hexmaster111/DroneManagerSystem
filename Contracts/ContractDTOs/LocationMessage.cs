using DroneManager.Interface.GenericTypes.BaseTypes;
using GenericMessaging;
using Newtonsoft.Json.Linq;

namespace Contracts.ContractDTOs;

public class LocationMessage : SenableDtoBase
{
    public LocationMessage(Location location)
    {
        Location = location;
    }

    public Location Location { get; set; }


}