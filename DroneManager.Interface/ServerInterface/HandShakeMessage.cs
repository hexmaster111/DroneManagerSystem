using System.Text.Json;
using DroneManager.Interface.GenericTypes;
using Newtonsoft.Json.Linq;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace DroneManager.Interface.ServerInterface;

public class HandShakeMessage : ISendable
{
    public HandShakeMessage(DroneId id)
    {
        Id = id;
        TimeStamp = DateTime.Now;
    }


    [JsonConstructor]
    public SendableTarget(string targetInfo, byte[] containedClass)
    {
        TargetInfo = targetInfo;
        this.containedClass = containedClass;
    }

    public DroneId Id { get; set; }
    public DateTime TimeStamp { get; set; }


    public JObject ToJson()
    {
        return JObject.FromObject(this);
    }
};

