using Contracts;
using GenericEventMapper;

namespace ServerBackend;

public class ClientEndpointContractImpl : ClientEndpointContract
{
    private readonly EventMapper _eventMapper;
    private readonly IConsoleLog.IConsoleLog _consoleLog;

    public ClientEndpointContractImpl(ref EventMapper eventMapper, IConsoleLog.IConsoleLog consoleLog)
    {
        _eventMapper = eventMapper;
        _consoleLog = consoleLog;
    }

    public override void RefreshReceivingContract()
    {
        GenericEventMapper.ReceivingContractRegister.RegisterContracts(_eventMapper, this, _consoleLog);
    }
}

public class ServerEndpointContractImpl : ServerEndpointContract
{
    public override void RefreshReceivingContract()
    {
        throw new NotImplementedException();
    }
}