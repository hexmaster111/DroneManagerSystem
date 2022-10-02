using DroneManager.Interface.Remote;
using GenericMessaging;
using Newtonsoft.Json.Linq;

namespace Contracts.ContractDTOs;

public class VitalsUpdateMessage : ISendable
{
    public VitalsUpdateMessage(double temperature, double heartRate, double breathingRate)
    {
        Temperature = temperature;
        HeartRate = heartRate;
        BreathingRate = breathingRate;
    }

    public double Temperature { get; }
    public double HeartRate { get; }
    public double BreathingRate { get; }


    public JObject ToJson()
    {
        return JObject.FromObject(this);
    }
}