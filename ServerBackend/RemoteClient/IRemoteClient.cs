using Contracts;
using DroneManager.Interface.GenericTypes.BaseTypes;

namespace ServerBackend.RemoteClient;

public interface IRemoteClient
{
    public ServerEndpointContract ReceivingContract { get; }
    public ClientEndpointContract SendingContract { get; }
    public bool IsConnected { get; }
    public Action<ConnectionStatus> OnConnectionStatusChanged { get; set; }
    public IRemoteClientNetworkInfo NetworkInformation { get; }
}