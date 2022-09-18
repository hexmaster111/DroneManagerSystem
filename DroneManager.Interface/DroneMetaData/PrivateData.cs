namespace DroneManager.Interface.DroneMetaData;

public abstract class PrivateData
{
    //Object is a placeholder for whatever code is needed to de-crypt the medical data
    public abstract bool ReadMedicalData(object code, out MedicalData medicalData);
    public abstract bool WriteMedicalData(object code, MedicalData data);
}