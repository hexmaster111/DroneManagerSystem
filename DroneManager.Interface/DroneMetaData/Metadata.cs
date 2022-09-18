namespace DroneManager.Interface.DroneMetaData;

//Todo Add a do not send to drone flag to the meta data
public class Metadata
{
    public KinkInfo KinkInfo { get; set; }
    public DateTime ActivationTime { get; set; }
    public string PersonalityDescription { get; set; }
    public string PastName { get; set; }
    
    public PrivateData PrivateData { get; set; }
}