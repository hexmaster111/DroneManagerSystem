using Contracts.ContractDTOs;
using GenericEventMapper;

namespace Contracts;

public abstract class ClientEndpointContract : ContractBase //Things the client can receive and the server can send
{
    public ContractItem<HandShakeMessage> HandShake { get; } = new TcpContractItem<HandShakeMessage>();
    public ContractItem<ChatMessage> BroadcastChatMessage { get; } = new TcpContractItem<ChatMessage>();
    public ContractItem<BlankRequest> HardwareUpdateRequest { get; } = new TcpContractItem<BlankRequest>();
    public ContractItem<SetRegisterMessage> SetRegister { get; } = new TcpContractItem<SetRegisterMessage>();
}