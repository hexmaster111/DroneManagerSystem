using Contracts.ContractDTOs;

namespace ServerBackend.RemoteClient;

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


        RemoteClient.ReceivingContract.InitialConnectionHandShake.Action = null;
        RemoteClient.ReceivingContract.RefreshReceivingContract();
        
        //RemoteClient.ClientEndpoint.RefreshReceivingContract();

        var drone = new DroneClient(RemoteClient);
        drone.SetDroneId(obj.Id);
        drone.RemoteClient = RemoteClient;
        OnRegister(drone, this);
    }

    public RemoteClient RemoteClient { get; set; }
}
