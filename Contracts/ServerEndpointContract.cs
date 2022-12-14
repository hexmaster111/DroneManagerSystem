using Contracts.ContractDTOs;
using GenericEventMapper;

namespace Contracts;

public abstract class ServerEndpointContract : ContractBase //Things the server can receive and the client can send
{
    protected ServerEndpointContract(EventMapper eventMapper)
    {
        EventMapper = eventMapper;
    }

    public ContractItem<HandShakeMessage> InitialConnectionHandShake { get; } = new TcpContractItem<HandShakeMessage>();
    public ContractItem<VitalsUpdateMessage> VitalsUpdate { get; } = new TcpContractItem<VitalsUpdateMessage>();
    public ContractItem<LocationMessage> LocationUpdate { get; } = new TcpContractItem<LocationMessage>();
    public ContractItem<HardwareInfoUpdateMessage> HardwareInfoUpdate { get; } = new TcpContractItem<HardwareInfoUpdateMessage>();
    
    public ContractItem<HeartBeatSuperMessage> HeartBeat { get; } = new TcpContractItem<HeartBeatSuperMessage>();

    public EventMapper EventMapper { get; }

}