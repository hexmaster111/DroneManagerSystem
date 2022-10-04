using Contracts;
using GenericEventMapper;

namespace ServerBackend.RemoteClient;

public class ServerEndpointContractImpl : ServerEndpointContract
{
    private readonly EventMapper _eventMapper;
    private readonly IConsoleLog.IConsoleLog _consoleLog;

    public ServerEndpointContractImpl(ref EventMapper eventMapper, IConsoleLog.IConsoleLog consoleLog) : base(eventMapper)
    {
        _eventMapper = eventMapper;
        _consoleLog = consoleLog;
    }
    public override void RefreshReceivingContract()
    {
        GenericEventMapper.ReceivingContractRegister.RegisterContracts(_eventMapper, this, _consoleLog);
    }
}