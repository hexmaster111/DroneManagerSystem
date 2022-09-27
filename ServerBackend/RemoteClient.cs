using System.Net.Sockets;
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
    private readonly EventMapper _eventMapper;
    private IConsoleLog.IConsoleLog? _log;

    public RemoteClient(TcpClient client, IConsoleLog.IConsoleLog? log = null)
    {
        _client = client;

        _log = log;

        _reader = new GenericReader(client.GetStream());
        _writer = new GenericWriter(client.GetStream());
        _eventMapper = new EventMapper(log);

        _mapEvents();
        _reader.OnMessageReceived += _eventMapper.HandleEvent;

        _reader.StartReading();
        _log.WriteLog(message:"Trying to send data", logLevel:LogLevel.Debug);
        _writer.SendData(new SendableTarget(new HandShakeMessage(new DroneId(DroneType.Experimental, 5050)),
            "HandShake"));
    }

    private void _mapEvents()
    {
        _eventMapper.MapAction<HandShakeMessage>("Handshake",
            (message) =>
            {
                _log?.WriteLog(
                    message: $"Received TestHandshake from {message.Id} sent at {message.TimeStamp}",
                    logLevel: LogLevel.Info);
            });
    }
}