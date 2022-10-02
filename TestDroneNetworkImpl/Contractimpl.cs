using Contracts;
using GenericEventMapper;

namespace TestDroneNetworkImpl;

public class ServerEndpointContractImpl : ServerEndpointContract
{
    public override void RefreshReceivingContract()
    {
        throw new NotImplementedException();
    }

    public ServerEndpointContractImpl(EventMapper eventMapper) : base(eventMapper)
    {
    }
}

public class ClientEndpointContractImpl : ClientEndpointContract
{
    public override void RefreshReceivingContract()
    {
        throw new NotImplementedException();
    }
}