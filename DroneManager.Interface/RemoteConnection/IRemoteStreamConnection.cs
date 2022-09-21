using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.ServerInterface;

namespace DroneManager.Interface.RemoteConnection;

public interface IRemoteStreamConnection
{
    public void Disconnect(ISendable? disconnectionArgs);
    public void Connect(object? connectionArgs);
    public void SendData(ISendable data);

    public event Action<ISendable> DataReceived;
    public event Action<ISendable> ConnectionStatusChanged;

    public ConnectionType ConnectionType { get; }
    public ConnectionStatus Status { get; }
}