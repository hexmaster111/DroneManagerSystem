using DroneManager.Interface.GenericTypes.BaseTypes;

namespace DroneManager.Interface.RemoteConnection;

public interface IRemoteStreamConnection 
{
    public void Disconnect(object? disconnectionArgs);
    public void Connect(object? connectionArgs);
    public event Action<object> DataReceived;
    public event Action<object> DataSent;
    public event Action<object> ConnectionStatusChanged;
    
    
    public ConnectionType ConnectionType { get; }
    public ConnectionStatus Status { get; }
}