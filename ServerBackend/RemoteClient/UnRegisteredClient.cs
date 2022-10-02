using Contracts.ContractDTOs;

namespace ServerBackend;

public class UnRegisteredClient
{
    public UnRegisteredClient(RemoteClient remoteClient, Action<DroneClient, object> onRegister)
    {
        OnRegister = onRegister;
        remoteClient.ReceivingContract.InitialConnectionHandShake.Action = OnHandshake;
        remoteClient.ReceivingContract.RefreshReceivingContract();
        RemoteClient = remoteClient;
    }

    private Action<DroneClient, object> OnRegister { get; }

    private void OnHandshake(HandShakeMessage obj)
    {
        RemoteClient.ReceivingContract.EventMapper.UnregisterAction(nameof(RemoteClient.ReceivingContract
            .InitialConnectionHandShake));


        //RemoteClient.ClientEndpoint.RefreshReceivingContract();

        var drone = new DroneClient(RemoteClient);
        drone.RemoteClient = RemoteClient;
        drone.SetDroneId(obj.Id);
        OnRegister(drone, this);
    }

    public RemoteClient RemoteClient { get; set; }
}
