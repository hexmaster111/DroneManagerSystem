namespace DroneManager.Interface.DroneMetaData;

//Todo Add a do not send to drone flag to the meta data
public abstract class Metadata
{
    public string PastName { get; set; }
    public string SaveWords { get; set; }
    public string Limits { get; set; }
    public string Notes { get; set; }
    public string ID { get; set; } // Not sure if this should ever be changed
    public DateTime ActivationTime { get; set; }
    public string PersonalityDescription { get; set; }

    //Object is a placeholder for whatever code is needed to de-crypt the medical data
    public abstract bool GetMedicalData(object code, out MedicalData medicalData);
    public abstract bool SetMedicalData(object code, MedicalData data);
}