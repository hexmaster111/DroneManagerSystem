using Contracts.ContractDTOs;
using CrappyLicenseTool;
using GenericEventMapper;

namespace Contracts;

public abstract class ServerEndpointContract : ContractBase //Things the server can receive and the client can send
{
    public ContractItem<HandShakeMessage> InitialConnectionHandShake { get; } = new TcpContractItem<HandShakeMessage>();

}