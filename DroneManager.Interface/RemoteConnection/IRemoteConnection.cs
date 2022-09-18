using DroneManager.Interface.GenericTypes.BaseTypes;

namespace DroneManager.Interface.RemoteConnection;

public interface IRemoteConnection
{
    public ConnectionType ConnectionType { get; }
    public ConnectionStatus Status { get; }
    public void Connect(object? connectionArgs);
    public void Disconnect(object? disconnectionArgs);
    public bool SendData(object data); //Object should be a serializable type
}