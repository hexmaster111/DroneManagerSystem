using DroneManager.Interface.GenericTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DroneManager.Interface.ServerInterface;

public class HandShakeMessage : ISendable
{
    public HandShakeMessage(DroneId id)
    {
        Id = id;
        TimeStamp = DateTime.Now;
    }


    [JsonConstructor]
    HandShakeMessage(DroneId id, DateTime time)
    {
        Id = id;
        TimeStamp = time;
    }


    public DroneId Id { get; set; }
    public DateTime TimeStamp { get; set; }


    public JObject ToJson()
    {
        return JObject.FromObject(this);
    }
};