namespace DroneManager.Interface.Remote;

public interface IVital
{
    public double Temperature { get; }
    public double HeartRate { get; }
    public double BreathingRate { get; }
}