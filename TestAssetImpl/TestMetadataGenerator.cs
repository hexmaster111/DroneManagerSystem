using DroneManager.Interface.DroneMetaData;

namespace TestAssetImpl;

public static class TestMetadataGenerator
{
    public static Metadata Generate()
    {
        return new Metadata
        {
            ActivationTime = DateTime.Now - TimeSpan.FromDays(10),
            KinkInfo = new()
            {
                Limits = "Things and stuff",
                Notes = "Dose things and stuff well",
                SaveWords = "Red, Yellow, Green"
            },
            PastName = "Old Name",
            PersonalityDescription = "Does things and stuff",
            PrivateData = PrivateDataGenerator.Generate(),
            
        };
    }

    private static class PrivateDataGenerator
    {
        public static PrivateData Generate()
        {
            return new PrivateDataImpl();
        }
        
        private class PrivateDataImpl : PrivateData
        {
            
            public byte[] Data { get; } = new byte[1000];
            public override bool ReadMedicalData(object code, out MedicalData medicalData)
            {
                medicalData = new MedicalData()
                {
                    MedicalConditions = new[] { "Condition 1", "Condition 2" },
                    MedicalMedications = new[] { "Medication 1", "Medication 2" },
                    MedicalNotes = "Notes",
                };
                return true;
            }

            public override bool WriteMedicalData(object code, MedicalData data)
            {
                throw new NotImplementedException();
            }
        }
    }
}