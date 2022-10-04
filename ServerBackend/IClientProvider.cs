namespace ServerBackend;

public interface IClientProvider
{
    // public IRemoteClient[] RemoteClients { get; }
    public Action<RemoteClient.RemoteClient> OnClientConnected { get; set; }
}