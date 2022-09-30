namespace DroneManager.Interface.DroneMetaData;

public class Metadata
{
    public KinkInfo KinkInfo { get; set; }
    public DateTime ActivationTime { get; set; }
    public string PersonalityDescription { get; set; }
    public string PastName { get; set; }
    
    public PrivateData PrivateData { get; set; }
}