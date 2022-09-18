namespace DroneManager.Interface.GenericTypes;

public class DroneId
{
    private static readonly string _separator = "-";
    private static readonly string _prefix = "WTD";
    private DroneType _Type { get; }
    private int _Id { get; }


    private static string _TypeToString(DroneType type)
    {
        return type switch
        {
            DroneType.DirectorAssistant => "DA",
            DroneType.SeniorAssistant => "SA",
            DroneType.ResearchAndDevelopment => "RD",
            DroneType.Experimental => "EX",
            DroneType.HiveMaintenance => "HM",
            DroneType.ProgramRecruiting => "PR",
            DroneType.ServiceModel => "SM",
            DroneType.DomesticService => "DS",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
    
    
    public DroneId(DroneType type, int id)
    {
        _Type = type;
        _Id = id;
    }

    public override string ToString()
    {
        return $"{_prefix}{_separator}{_TypeToString(_Type)}{_separator}{_Id}";
    }
}