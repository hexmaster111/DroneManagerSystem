using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using IConsoleLogInterface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Server;

/// <summary>
///     Singleton
/// </summary>
public class ServerBackend
{
    private static ServerBackend _instance;
    public static ServerBackend Instance => _instance ??= new ServerBackend();

    /// <summary>
    ///    The server's Listener port
    /// </summary>
    private int _serverPort;

    private string _localIp;
    private TcpListener? _server = null;


    public static IConsoleLogInterface.IConsoleLog ConsoleLog { get; set; }

    public void Start(string localIp, int serverPort)
    {
        if (ConsoleLog == null)
            throw new Exception("ConsoleLog is null");
        _serverPort = serverPort;
        _localIp = localIp;
        ConsoleLog.WriteLog(message: "Starting Listener...", logLevel: LogLevel.Info);
        var listenerThread = new Thread(_listenerThread);
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
        _clients.Add(new RemoteClient(client, ConsoleLog));
    }
}