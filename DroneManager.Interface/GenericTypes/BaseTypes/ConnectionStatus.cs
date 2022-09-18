namespace DroneManager.Interface.GenericTypes.BaseTypes;

public enum ConnectionStatus
{
    NotTried, // Default value
    Connected,
    Connecting,
    Disconnected,
    Disconnecting,
    Error
}