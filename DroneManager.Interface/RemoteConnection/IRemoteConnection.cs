using DroneManager.Interface.GenericTypes;

namespace DroneManager.Interface.RemoteConnection;

public interface IRemoteConnection
{
    public ConnectionType ConnectionType { get; }
    public ConnectionStatus Status { get; }
    public void Connect(object connectionArgs);
    public void Disconnect();
    public bool SendData(object data); //Object should be a class that implements IRemoteSendable
}