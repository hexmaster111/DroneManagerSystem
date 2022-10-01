using Contracts.ContractDTOs;

namespace ServerBackend;

public class UnRegisteredClient
{
    public UnRegisteredClient(IRemoteClient remoteClient, Action<DroneCommunicationLayerAbstraction, object> onRegister)
    {
        OnRegister = onRegister;
        remoteClient.ClientEndpoint.InitialConnectionHandShake.Action += OnHandshake;
        remoteClient.ClientEndpoint.RefreshReceivingContract();
        RemoteClient = remoteClient;
    }

    private Action<DroneCommunicationLayerAbstraction, object> OnRegister { get; }

    private void OnHandshake(HandShakeMessage obj)
    {
        RemoteClient.ClientEndpoint.InitialConnectionHandShake.Action -= OnHandshake;
        RemoteClient.ClientEndpoint.RefreshReceivingContract();

        var drone = new DroneCommunicationLayerAbstraction();
        drone.RemoteClient = RemoteClient;
        drone.Id = obj.Id;
        OnRegister(drone, this);
    }

    public IRemoteClient RemoteClient { get; set; }
}