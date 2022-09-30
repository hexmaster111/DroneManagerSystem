using System.Net.Sockets;
using Contracts;
using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.ServerInterface;
using GenericEventMapper;
using GenericMessaging;
using IConsoleLog;

namespace ServerBackend;

public class RemoteClient
{
    private TcpClient _client;
    private readonly NetworkStream _stream;
    private readonly Thread _thread;

    private GenericReader _reader;
    private GenericWriter _writer;
    private EventMapper _eventMapper;
    private IConsoleLog.IConsoleLog? _log;

    private ServerEndpointContract _serverEndpointContract = new(); //Out from the server to the client
    private ClientEndpointContract _clientEndpointContract = new(); //In from the client to the server

    public RemoteClient(TcpClient client, IConsoleLog.IConsoleLog? log = null)
    {
        _client = client;

        _log = log;

        _reader = new GenericReader(client.GetStream());
        _writer = new GenericWriter(client.GetStream());
        _eventMapper = new EventMapper(log);

        _mapEvents();
        _setupSendingContract();
        _reader.OnMessageReceived += _eventMapper.HandleEvent;

        _reader.StartReading();
    }


    public void SendData(SendableTarget target)
    {
        _writer.SendData(target);
    }


    private void _setupSendingContract()
    {
        SendingContractRegister.RegisterSendingContract(_clientEndpointContract, new object[] {  _writer }, _log);
    }


    private void _mapEvents()
    {
        _serverEndpointContract.HandShake.Action += OnHandShake;
        _serverEndpointContract.HandShake2.Action += OnHandShake;
        ReceivingContractRegister.RegisterContracts(ref _eventMapper, _serverEndpointContract, _log);
    }

    private void OnHandShake(HandShakeMessage obj)
    {
        _log.WriteLog("Handshake received");
    }
}