using DroneManager.Interface.GenericTypes;
using GenericMessaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Contracts.ContractDTOs;

public class HandShakeMessage : SenableDtoBase
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
};