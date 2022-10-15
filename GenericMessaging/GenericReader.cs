using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GenericMessaging;

public class GenericReader
{
    private readonly NetworkStream _stream;
    private bool _reading;
    private Thread? _reader;
    public event Action<SendableTarget> OnMessageReceived;

    public GenericReader(NetworkStream stream)
    {
        _stream = stream;
    }

    public SendableTarget? ReadData()
    {
        var buffer = new byte[100_000];
        var read = _stream.Read(buffer, 0, buffer.Length);
        var data = Encoding.UTF8.GetString(buffer, 0, read);
        var message = JObject.Parse(data);
        var target = message.ToObject<SendableTarget>();
        if (target?.TargetInfo == null) throw new("Target Null Exception");
        return target;
    }


    public void StartReading()
    {
        _reading = true;
        _reader = new Thread(ReadingThread);
        _reader.Start();
    }

    public void StopReading()
    {
        _reading = false;
    }


    private void ReadingThread()
    {
        while (_reading)
        {
            Thread.Sleep(50);
            if (!_stream.DataAvailable) continue;
            var data = ReadData();
            if (data == null) continue;
            OnOnMessageReceived(data);
        }
    }

    protected virtual void OnOnMessageReceived(SendableTarget obj)
    {
        OnMessageReceived?.Invoke(obj);
    }
}