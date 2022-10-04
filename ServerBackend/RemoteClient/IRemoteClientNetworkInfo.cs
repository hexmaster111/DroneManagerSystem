using System.Net;
using DroneManager.Interface.GenericTypes.BaseTypes;

namespace ServerBackend.RemoteClient;

public interface IRemoteClientNetworkInfo
{
    public ConnectionStatus ConnectionStatus { get; }
    public bool IsConnected { get; }
    public IPAddress ClientProviderAddress { get; }
    public int ClientProviderPort { get; }
    public DateTime LastMessage { get; }
}