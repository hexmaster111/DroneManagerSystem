namespace DroneManager.Interface.Remote;

public interface IVital
{
    public double Temperature { get; }
    public double HeartRate { get; }
    public double BreathingRate { get; }
    
    public double MaxTemperature { get; }
    public double MaxHeartRate { get; }
    public double MaxBreathingRate { get; }
    
    public double MinTemperature { get; }
    public double MinHeartRate { get; }
    public double MinBreathingRate { get; }
}