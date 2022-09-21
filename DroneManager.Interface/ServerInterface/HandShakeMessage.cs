using System.Text.Json;
using DroneManager.Interface.GenericTypes;

namespace DroneManager.Interface.ServerInterface;

public class HandShakeMessage : ISendable
{
    public HandShakeMessage(DroneId id)
    {
        Id = id;
        TimeStamp = DateTime.Now;
    }

    public DroneId Id { get; set; }
    public DateTime TimeStamp { get; set; }


    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
};

