using DroneManager.Interface.RemoteConnection;
using DroneManager.Interface.ServerInterface;
using IConsoleLog;

namespace ServerBackend;

public class UnRegisteredClient
{
    public UnRegisteredClient(IRemoteClient remoteClient, Action<DroneCommunicationLayerAbstraction, object> onRegister)
    {
        OnRegister = onRegister;
        remoteClient.ClientEndpoint.HandShake.Action += OnHandshake;
        remoteClient.ClientEndpoint.RefreshReceivingContract();
        RemoteClient = remoteClient;
    }

    private Action<DroneCommunicationLayerAbstraction, object> OnRegister { get; }

    private void OnHandshake(HandShakeMessage obj)
    {
        RemoteClient.ClientEndpoint.HandShake.Action -= OnHandshake;
        RemoteClient.ClientEndpoint.RefreshReceivingContract();

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
    
    private IConsoleLog.IConsoleLog _consoleLog;

    public RemoteClientManager(IClientProvider clientProvider, IConsoleLog.IConsoleLog consoleLog)
    {
        _clientProvider = clientProvider;
        _consoleLog = consoleLog;
        _clientProvider.OnClientConnected += OnClientConnected;
        _onClientRegistered += OnClientRegistered;
    }

    private void OnClientRegistered(DroneCommunicationLayerAbstraction obj, object sender)
    {
        // Rmeove the unregistered client from the list
        _unregisteredClients.Remove((UnRegisteredClient)sender);
        // Check if the client is already in the list
        if (_clients.Contains(obj))
        {
            // check for clients with the same ID and remove them
            var clientsWithSameId = _clients.Where(x => x.Id == obj.Id).ToList();
            foreach (var client in clientsWithSameId)
            {
                _clients.Remove(client);
            }
        }
        
        _consoleLog.WriteLog($"Drone {obj.Id} connected", LogLevel.Notice);

        // Add the client to the list
        _clients.Add(obj);
    }


    private void OnClientConnected(IRemoteClient obj)
    {
        //Add the client to a list of clients who have not yet given the handshakeInfo
        var client = new UnRegisteredClient(obj, _onClientRegistered);
        _unregisteredClients.Add(client);
    }
}