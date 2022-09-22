using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using DroneManager.Interface.ServerInterface;
using IConsoleLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ServerBackend;

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


    public static IConsoleLog.IConsoleLog ConsoleLog { get; set; }

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
        _clients.Add(new RemoteClient(client));
    }


    public class RemoteClient
    {
        private TcpClient _client;
        private readonly NetworkStream _stream;
        private readonly Thread _thread;

        public RemoteClient(TcpClient client)
        {
            _client = client;
            _stream = client.GetStream();
            _thread = new Thread(_clientReadThread);
            _thread.Start();
        }

        private void _clientReadThread()
        {
            while (true)
            {
                if (_stream.DataAvailable)
                {
                    var buffer = new byte[1024];
                    var bytesRead = _stream.Read(buffer, 0, buffer.Length);
                    var data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    var message = JObject.Parse(data);
                    var a = message.ToObject<SendableTarget>();
                    
                    var classInside = Encoding.Unicode.GetString(a.ContainedClass);
                    
                    
                    ConsoleLog.WriteLog(message: $"Received: {a.TargetInfo}", logLevel: LogLevel.Info);
                    //ConsoleLog.WriteLog(message: $"Connection From: {a.Id}", logLevel: LogLevel.Notice);
                }

                
                
                
                Thread.Sleep(100);
            }
        }
        

        private void _handleHandshakeMessage(HandShakeMessage message)
        {
        }
    }
}