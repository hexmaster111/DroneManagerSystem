namespace ServerBackend.Abstraction;

public interface IRemoteClientManager
{
    /// <summary>
    /// Action thrown when a new client provides a handshake and is accepted
    /// </summary>
    public Action<DroneClient> OnConnectedClient { get; set; }
    public Action<DroneClient> OnDisconnectedClient { get; set; }
}