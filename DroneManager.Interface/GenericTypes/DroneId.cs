using System.Diagnostics;

namespace DroneManager.Interface.GenericTypes;

public class DroneId
{
    private static readonly string _separator = "-";
    private static readonly string _prefix = "WTD";
    public DroneType Type { get; }
    public int Id { get; }


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

    public override bool Equals(object? obj)
    {
        var id = obj as DroneId;
        var same = id != null &&
                   Type == id.Type &&
                   Id == id.Id;

        return same;
    }

    private bool Equals(DroneId other)
    {
        return Type == other.Type && Id == other.Id;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Type, Id);
    }

    public DroneId(DroneType type, int id)
    {
        Type = type;
        Id = id;
    }

    public override string ToString()
    {
        return $"{_prefix}{_separator}{Id}{_separator}{_TypeToString(Type)}";
    }
}