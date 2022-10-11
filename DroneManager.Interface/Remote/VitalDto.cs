namespace DroneManager.Interface.Remote;

public class VitalDto
{
    public double Temperature { get; set; } = 0;
    public double HeartRate { get; set; } = 0;
    public double BreathingRate { get; set; } = 0;
    
    public double MaxTemperature { get; } = Double.NaN;
    public double MaxHeartRate { get; } = Double.NaN;
    public double MaxBreathingRate { get; } = Double.NaN;
    
    public double MinTemperature { get; } = Double.NaN;
    public double MinHeartRate { get; } = Double.NaN;
    public double MinBreathingRate { get; } = Double.NaN;
}