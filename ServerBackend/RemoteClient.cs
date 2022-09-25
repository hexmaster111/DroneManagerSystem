using System.Net.Sockets;
using ConsoleLogging;
using DroneManager.Interface.ServerInterface;
using GenericEventMapper;
using GenericMessaging;
using IConsoleLogInterface;

namespace Server;

public class RemoteClient
{
    private TcpClient _client;
    private readonly NetworkStream _stream;
    private readonly Thread _thread;

    private GenericReader _reader;
    private GenericWriter _writer;
    private readonly EventMapper _eventMapper;
    private IConsoleLog? _log;

    public RemoteClient(TcpClient client, IConsoleLog? log = null)
    {
        _client = client;
        
        _log = log;

        _reader = new GenericReader(client.GetStream());
        _writer = new GenericWriter(client.GetStream());
        _eventMapper = new EventMapper(log);

        _mapEvents();
        _reader.OnMessageReceived += _eventMapper.HandleEvent;

        _reader.StartReading();
    }

    private void _mapEvents()
    {
        _eventMapper.MapAction<HandShakeMessage>("TestHandshake",
            (message) =>
            {
                _log?.WriteLog(
                    message: $"Received TestHandshake from {message.Id} sent at {message.TimeStamp}",
                    logLevel: LogLevel.Info);
            });
    }
}