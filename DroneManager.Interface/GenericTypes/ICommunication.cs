namespace DroneManager.Interface.GenericTypes;

public interface ICommunication
{
    string Name { get; set; }
    string Description { get; set; }
    ConnectionStatus Status { get; } 
    
}