namespace DroneManager.Interface.GenericTypes.BaseTypes;

public class Location
{
    public double Latitude { get; }
    public double Longitude { get; }
    public double Speed { get; }
    public double Timestamp { get; }
    public string LocationProvider { get; }
    public string? LocationName { get; }
    public string? LocationAddress { get; }
}