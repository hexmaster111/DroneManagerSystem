using Contracts.ContractDTOs;

namespace ServerBackend;

public class UnRegisteredClient
{
    public UnRegisteredClient(IRemoteClient remoteClient, Action<DroneClient, object> onRegister)
    {
        OnRegister = onRegister;
        remoteClient.ClientEndpoint.InitialConnectionHandShake.Action = OnHandshake;
        remoteClient.ClientEndpoint.RefreshReceivingContract();
        RemoteClient = remoteClient;
    }

    private Action<DroneClient, object> OnRegister { get; }

    private void OnHandshake(HandShakeMessage obj)
    {
        RemoteClient.ClientEndpoint.EventMapper.UnregisterAction(nameof(RemoteClient.ClientEndpoint
            .InitialConnectionHandShake));


        //RemoteClient.ClientEndpoint.RefreshReceivingContract();

        var drone = new DroneClient();
        drone.RemoteClient = RemoteClient;
        drone.SetDroneId(obj.Id);
        OnRegister(drone, this);
    }

    public IRemoteClient RemoteClient { get; set; }
}
