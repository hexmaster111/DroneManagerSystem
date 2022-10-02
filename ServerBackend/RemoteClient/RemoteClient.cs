using System.Net;
using System.Net.Sockets;
using Contracts;
using Contracts.ContractDTOs;
using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.RemoteConnection;
using GenericEventMapper;
using GenericMessaging;
using IConsoleLog;

namespace ServerBackend;

public interface IRemoteClient
{
    public ServerEndpointContract ClientEndpoint { get; }
    public bool IsConnected { get; }
    public Action<ConnectionStatus> OnConnectionStatusChanged { get; set; }
    public IRemoteClientNetworkInfo NetworkInformation { get; }
}

public interface IRemoteClientNetworkInfo
{
    public ConnectionStatus ConnectionStatus { get; }
    public bool IsConnected { get; }
    public IPAddress ClientProviderAddress { get; }
    public int ClientProviderPort { get; }
    public DateTime LastMessage { get; }
}

public class RemoteClient : IRemoteClient, IRemoteClientNetworkInfo
{
    private TcpClient? _client;
    private readonly NetworkStream _stream;
    private readonly Thread _thread;

    private GenericReader _reader;
    private GenericWriter _writer;
    private EventMapper _eventMapper;
    private IConsoleLog.IConsoleLog? _log;

    private ServerEndpointContract _serverEndpointContract;
    private ClientEndpointContract _clientEndpointContract;


    public RemoteClient(TcpClient client, IConsoleLog.IConsoleLog? log = null)
    {
        _client = client;
        _log = log;

        _reader = new GenericReader(client.GetStream());
        _writer = new GenericWriter(client.GetStream());
        _eventMapper = new EventMapper(log);
        _serverEndpointContract = new ServerEndpointContractImpl(ref _eventMapper, log);
        _clientEndpointContract = new ClientEndpointContractImpl();

        _serverEndpointContract.RefreshReceivingContract();
        _setupSendingContract();
        _reader.OnMessageReceived += _eventMapper.HandleEvent;
        _reader.OnMessageReceived += ReaderOnOnMessageReceived;


        _reader.StartReading();
    }

    public DateTime LastMessage { get; private set; }
    private void ReaderOnOnMessageReceived(SendableTarget _)
    {
        LastMessage = DateTime.Now;
    }


    public void SendData(SendableTarget target)
    {
        try
        {
            _writer.SendData(target);
        }
        //Catch the unable to write exception and send the new connection Action
        catch (Exception e)
        {
            _log?.WriteLog("Client disconnected");
            _client?.Close();
            _client = null;
            OnConnectionStatusChanged?.Invoke(ConnectionStatus.Disconnected);
        }
    }

    private void _setupSendingContract()
    {
        SendingContractRegister.RegisterSendingContract(_clientEndpointContract, new object[] { _writer }, _log);
    }


    public ServerEndpointContract ClientEndpoint => _serverEndpointContract;

    public ConnectionStatus ConnectionStatus =>
        _client == null ? ConnectionStatus.Disconnected : ConnectionStatus.Connected;

    public bool IsConnected => _client.Connected;
    public IPAddress ClientProviderAddress => ((IPEndPoint)_client.Client.RemoteEndPoint).Address;
    public int ClientProviderPort => ((IPEndPoint)_client.Client.RemoteEndPoint).Port;
    public Action<ConnectionStatus> OnConnectionStatusChanged { get; set; }
    public IRemoteClientNetworkInfo NetworkInformation => this;
}