namespace DroneManager.Interface.Remote;

public class VitalDto
{
    public double Temperature { get; init; }
    public double HeartRate { get; init; }
    public double BreathingRate { get; init; }
    
    public double MaxTemperature { get; } = Double.NaN;
    public double MaxHeartRate { get; } = Double.NaN;
    public double MaxBreathingRate { get; } = Double.NaN;
    
    public double MinTemperature { get; } = Double.NaN;
    public double MinHeartRate { get; } = Double.NaN;
    public double MinBreathingRate { get; } = Double.NaN;
}