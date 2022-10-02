using DroneManager.Interface.GenericTypes.BaseTypes;
using GenericMessaging;
using Newtonsoft.Json.Linq;

namespace Contracts.ContractDTOs;

public class LocationMessage : ISendable
{
    public LocationMessage(Location location)
    {
        Location = location;
    }

    public Location Location { get; set; }

    public JObject ToJson()
    {
        return JObject.FromObject(this);
    }
}