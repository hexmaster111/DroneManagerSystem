using System.Net.Sockets;
using Contracts;
using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.ServerInterface;
using GenericEventMapper;
using GenericMessaging;
using IConsoleLog;

namespace ServerBackend;

public interface IRemoteClient
{
    public ClientEndpointContract ClientEndpoint { get; }
    public bool IsConnected { get; }
    public Action<ConnectionStatus> OnConnectionStatusChanged { get; set; }
}

public class RemoteClient : IRemoteClient
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
        _serverEndpointContract = new ServerEndpointContractImpl();
        _clientEndpointContract = new ClientEndpointContractImpl(ref _eventMapper, log);

        _mapEvents();
        _setupSendingContract();
        _reader.OnMessageReceived += _eventMapper.HandleEvent;


        _reader.StartReading();
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


    private bool refreshContracts()
    {
        ReceivingContractRegister.RegisterContracts(_eventMapper, _serverEndpointContract, _log);
        return true;
    }
    
    private void _mapEvents()
    {
        _serverEndpointContract.HandShake.Action += OnHandShake;
        _serverEndpointContract.HandShake2.Action += OnHandShake;
        refreshContracts();
    }

    private void OnHandShake(HandShakeMessage obj)
    {
        _log?.WriteLog("Handshake received");
    }

    public ClientEndpointContract ClientEndpoint => _clientEndpointContract;
    public bool IsConnected => _client.Connected;
    public Action<ConnectionStatus> OnConnectionStatusChanged { get; set; }
}