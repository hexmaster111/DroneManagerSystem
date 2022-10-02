namespace DroneManager.Interface.GenericTypes.BaseTypes;

public class Location
{

    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public double Speed { get; init; }
    public DateTime TimeStamp { get; init; }
    public string LocationProvider { get; init; }
    public string? LocationName { get; init; }
    public string? LocationAddress { get; init; }
}