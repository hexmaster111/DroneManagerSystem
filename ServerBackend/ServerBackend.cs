using System.Net;
using System.Net.Sockets;
using ConsoleCommandHandler;
using ConsoleCommandHandler.Commands;
using Contracts;
using IConsoleLog;

namespace ServerBackend;

public interface IClientProvider
{
    // public IRemoteClient[] RemoteClients { get; }
    public Action<IRemoteClient> OnClientConnected { get; set; }
}


/// <summary>
///     Singleton
/// </summary>
public class ServerBackend : IClientProvider
{
    private static ServerBackend _instance;
    public static ServerBackend Instance => _instance ??= new ServerBackend();

    /// <summary>
    ///    The server's Listener port
    /// </summary>
    private int _serverPort;

    private string _localIp;
    private TcpListener? _server = null;

    public string LocalIp => _localIp;
    public int ServerPort => _serverPort;
    public bool IsRunning => listenerThread.IsAlive;

    public int ConnectedClients => _clients.Count;
    
    public RemoteClient[] Clients => _clients.ToArray();

    public static int MaxClients = 10;

    public static IConsoleLog.IConsoleLog ConsoleLog { get; set; }

    private Thread listenerThread;

    private ICommandAdder _commandAdder;

    public void Start(string localIp, int serverPort, ICommandAdder cliConnectionManager)
    {
        if (ConsoleLog == null)
            throw new Exception("ConsoleLog is null");

        _commandAdder = cliConnectionManager;

        _serverPort = serverPort;
        _localIp = localIp;
        ConsoleLog.WriteLog(message: "Starting Listener...", logLevel: LogLevel.Info);
        listenerThread = new Thread(_listenerThread);
        listenerThread.Start();
    }

    private void _listenerThread()
    {
        _server = null;
        try
        {
            if (!IPAddress.TryParse(_localIp, out var ip))
            {
                ConsoleLog.WriteLog(message: "Invalid IP address", logLevel: LogLevel.Fatal);
                return;
            }

            _server = new TcpListener(ip, _serverPort);
            _server.Start();


            while (true)
            {
                HandleClient(_server.AcceptTcpClient());
                Thread.Sleep(100);
            }
        }
        catch (SocketException e)
        {
            ConsoleLog.WriteLog(message: $"Server Exception: {e}", logLevel: LogLevel.Fatal);
        }
        finally
        {
            ConsoleLog.WriteLog(message: "Server Stopped", logLevel: LogLevel.Info);
            _server?.Stop();
        }
    }

    private List<RemoteClient> _clients = new();

    private void HandleClient(TcpClient client)
    {
        // print client info
        ConsoleLog.WriteLog(message: $"Client connected!", logLevel: LogLevel.Info);
        ConsoleLog.WriteLog(message: $"Client IP: {client.Client.RemoteEndPoint}", logLevel: LogLevel.Info);

        var remoteClient = new RemoteClient(client, ConsoleLog);

        OnClientConnected?.Invoke(remoteClient);

        _clients.Add(remoteClient);
    }

    public IRemoteClient[] RemoteClients => Clients;
    public Action<IRemoteClient> OnClientConnected { get; set; }
}