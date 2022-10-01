using DroneManager.Interface.ServerInterface;
using GenericEventMapper;

namespace Contracts;

public abstract class ClientEndpointContract : ContractBase //Things the client can receive and the server can send
{
    public ContractItem<HandShakeMessage> HandShake { get; } = new TcpContractItem<HandShakeMessage>();
}