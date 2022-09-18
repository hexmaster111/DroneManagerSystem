namespace DroneManager.Interface.DroneMetaData;

public class MedicalData
{
    public string MedicalNotes { get; set; }
    public string[] MedicalConditions { get; set; } // Food allergies, etc
    public string[] MedicalMedications { get; set; } // Daily Needs / Weekly / Monthly, etc
}