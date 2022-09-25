namespace ConsoleCommandHandler.Commands;

public class Argument
{
    public enum CompleteHelperType
    {
        None,
        Namespace,
        TrueFalse,
    }

    public Argument(string name, string description, CompleteHelperType completeHelperType)
    {
        Name = name;
        Description = description;
        HelperType = completeHelperType;
    }

    public string Name { get; }
    public string Description { get; }
    public CompleteHelperType HelperType { get; }
}