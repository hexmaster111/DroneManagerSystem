using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.RemoteConnection;
using GraphicalConsole;
using IConsoleLog;
using ServerBackend.RemoteClient;

namespace ServerBackend;

public class RemoteClientManager : IRemoteClientManager, IRemoteClientManagerFacade
{
    private IClientProvider _clientProvider;

    private TapSynchronized<List<DroneClient>> _droneClients = new(new());
    //private List<UnRegisteredClient> _unregisteredClients = new();

    //Event handler for when a client sends its first hadshake
    private Action<DroneClient, object> _onClientRegistered;

    private IConsoleLog.IConsoleLog _consoleLog;

    private TapSynchronized<List<UnRegisteredClient>> _unregisteredClientsTap = new(new());

    public RemoteClientManager(IClientProvider clientProvider, IConsoleLog.IConsoleLog consoleLog)
    {
        _clientProvider = clientProvider;
        _consoleLog = consoleLog;
        _clientProvider.OnClientConnected += OnClientConnected;
        _onClientRegistered += OnClientRegistered;
    }


    private void OnClientRegistered(DroneClient obj, object sender)
    {
        _unregisteredClientsTap.WithValue<object>((ref List<UnRegisteredClient> list) =>
        {
            // Rmeove the unregistered client from the list
            list.Remove((UnRegisteredClient)sender);
            return null;
        });

        _droneClients.WithValue<object>((ref List<DroneClient> _clients) =>
        {
            // Add the client to the list of registered clients
            _clients.Add(obj);

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
            return null;
        });
        // Call every who cares about the new client being 100% ready to go
        // Outside of the locked value to prevent deadlocks (Who knows what they will do to our presses clients)
        OnDroneConnected?.Invoke(obj.Id);
    }

    private void OnClientDisconnected(DroneClient obj)
    {
        OnDisconnectedClient?.Invoke(obj);
        _droneClients.WithValue<object>((ref List<DroneClient> _clients) =>
        {
            _clients.Remove(obj);
            return null;
        });
        _consoleLog.WriteLog($"{obj.Id} disconnected", LogLevel.Notice);


        ServerBackend.Instance.RemoveClient(obj.RemoteClient);
    }


    private void OnClientConnected(RemoteClient.RemoteClient obj)
    {
        //Add the client to a list of clients who have not yet given the handshakeInfo
        var client = new UnRegisteredClient(obj, _onClientRegistered);
        _unregisteredClientsTap.WithValue<object>((ref List<UnRegisteredClient> unRegisteredClients) =>
        {
            unRegisteredClients.Add(client);
            return null;
        });
    }


    public Action<DroneClient> OnConnectedClient { get; set; }
    public Action<DroneClient> OnDisconnectedClient { get; set; }


    #region IRemoteClientManagerFacade

    public Drone[] GetDrones()
    {
        return _droneClients.WithValue<Drone[]>((ref List<DroneClient> ctx) => ctx.ToArray()) ?? throw new
            InvalidOperationException();
    }

    public bool GetDrone(DroneId droneId, out Drone? drone)
    {
        drone = _droneClients.WithValue<Drone>((ref List<DroneClient> ctx) =>
        {
            var found = ctx.FirstOrDefault(x => Equals(x.Id, droneId));
            return found;
        });
        return drone != null;
    }


    public Action<DroneId> OnDroneConnected { get; set; }

    #endregion
}