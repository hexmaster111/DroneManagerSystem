using CrappyLicenseTool;
using DroneManager.Interface.ServerInterface;
using GenericEventMapper;

namespace Contracts;

public class ServerEndpointContract //Things the server can receive and the client can send
{
    public ContractItem<HandShakeMessage> HandShake { get; } = new TcpContractItem<HandShakeMessage>();
    public ContractItem<HandShakeMessage> HandShake2 { get; } = new TcpContractItem<HandShakeMessage>();

}