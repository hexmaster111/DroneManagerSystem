using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.Remote;
using DroneManager.Interface.RemoteConnection;
using DroneManager.Interface.ServerInterface;

namespace ServerBackend;

public class DroneCommunicationLayerAbstraction : IDrone
{
    public IRemoteClient? RemoteClient { get; set; }


    #region IDrone Members

    #region DroneId Implementation

    public DroneId Id { get; set; }

    #endregion


    public Location CurrentLocation { get; }
    public IVital Vitals { get; }
    public IControl Control { get; }

    #endregion
}

public class UnRegisteredClient
{
    public UnRegisteredClient(IRemoteClient remoteClient, Action<DroneCommunicationLayerAbstraction, object> onRegister)
    {
        OnRegister = onRegister;
        remoteClient.ClientEndpoint.HandShake.Action += OnHandshake;
        remoteClient.ClientEndpoint.HandShake.RefreshContract();
        RemoteClient = remoteClient;
    }

    private Action<DroneCommunicationLayerAbstraction, object> OnRegister { get; }

    private void OnHandshake(HandShakeMessage obj)
    {
        RemoteClient.ClientEndpoint.HandShake.Action -= OnHandshake;
        RemoteClient.ClientEndpoint.HandShake.RefreshContract();

        var drone = new DroneCommunicationLayerAbstraction();
        drone.RemoteClient = RemoteClient;
        drone.Id = obj.Id;
        OnRegister(drone, this);
    }

    public IRemoteClient RemoteClient { get; set; }
}

public class RemoteClientManager
{
    private IClientProvider _clientProvider;

    private List<DroneCommunicationLayerAbstraction> _clients = new();
    private List<UnRegisteredClient> _unregisteredClients = new();

    private Action<DroneCommunicationLayerAbstraction, object> _onClientRegistered;

    public RemoteClientManager(IClientProvider clientProvider)
    {
        _clientProvider = clientProvider;
        _clientProvider.OnClientConnected += OnClientConnected;
        _onClientRegistered += OnClientRegistered;
    }

    private void OnClientRegistered(DroneCommunicationLayerAbstraction obj, object sender)
    {
        // Rmeove the unregistered client from the list
        _unregisteredClients.Remove((UnRegisteredClient)sender);
        // Check if the client is already in the list
        if (!_clients.Contains(obj))
        {
            // Add the client to the list
            _clients.Add(obj);
        }

        // check for clients with the same ID and remove them
        var clientsWithSameId = _clients.Where(x => x.Id == obj.Id).ToList();
        foreach (var client in clientsWithSameId)
        {
            _clients.Remove(client);
        }

        // Add the new client to the list
        _clients.Add(obj);
    }


    private void OnClientConnected(IRemoteClient obj)
    {
        //Add the client to a list of clients who have not yet given the handshakeInfo
        var client = new UnRegisteredClient(obj, _onClientRegistered);
        _unregisteredClients.Add(client);
    }
}