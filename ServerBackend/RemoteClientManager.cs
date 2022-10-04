using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.RemoteConnection;
using IConsoleLog;
using ServerBackend.RemoteClient;

namespace ServerBackend;

public class RemoteClientManager : IRemoteClientManager
{
    private IClientProvider _clientProvider;

    private List<DroneClient> _clients = new();
    private List<UnRegisteredClient> _unregisteredClients = new();

    //Event handler for when a client sends its first hadshake
    private Action<DroneClient, object> _onClientRegistered;

    private IConsoleLog.IConsoleLog _consoleLog;

    public RemoteClientManager(IClientProvider clientProvider, IConsoleLog.IConsoleLog consoleLog)
    {
        _clientProvider = clientProvider;
        _consoleLog = consoleLog;
        _clientProvider.OnClientConnected += OnClientConnected;
        _onClientRegistered += OnClientRegistered;
    }

    private void OnClientRegistered(DroneClient obj, object sender)
    {
        // Rmeove the unregistered client from the list
        _unregisteredClients.Remove((UnRegisteredClient)sender);
        // Check if the client is already in the list
        var found = _clients.Any(client => Equals(client.Id, obj.Id));

        if (found)
        {
            // check for clients with the same ID and remove them
            var clientsWithSameId = _clients
                .Where(x => Equals(x.Id, obj.Id)).ToList();
            foreach (var client in clientsWithSameId)
            {
                _clients.Remove(client);
                _consoleLog.WriteLog($"{client.Id} was already registered, updated to new client");
            }
        }

        _consoleLog.WriteLog($"{obj.Id} connected", LogLevel.Notice);
        obj.OnConnect();
        obj.OnDisconnect += OnClientDisconnected;
        OnConnectedClient?.Invoke(obj);
        // Add the client to the list
        _clients.Add(obj);
    }
    
    private void OnClientDisconnected(DroneClient obj)
    {
        OnDisconnectedClient?.Invoke(obj);
        _clients.Remove(obj);
        _consoleLog.WriteLog($"{obj.Id} disconnected", LogLevel.Notice);

        
        
        ServerBackend.Instance.RemoveClient(obj.RemoteClient);
    }


    private void OnClientConnected(RemoteClient.RemoteClient obj)
    {
        //Add the client to a list of clients who have not yet given the handshakeInfo
        var client = new UnRegisteredClient(obj, _onClientRegistered);
        _unregisteredClients.Add(client);
    }

    
    public Action<DroneClient> OnConnectedClient { get; set; }
    public Action<DroneClient> OnDisconnectedClient { get; set; }
}