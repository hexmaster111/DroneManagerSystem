using System.Text;
using Newtonsoft.Json;

namespace GenericMessaging;

public class GenericWriter
{
    private readonly Stream _stream;

    public Action StreamClosed;

    public GenericWriter(Stream stream)
    {
        _stream = stream;
    }

    public void SendData(SendableTarget data)
    {
        var target = new SendableTarget(data.TargetInfo, data.ToJson());

        var send = target.ToJson();

        var buffer = Encoding.ASCII.GetBytes(send.ToString(Formatting.None));

        try
        {
            _stream.Write(buffer, 0, buffer.Length);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            StreamClosed?.Invoke();
            throw;
        }
    }
}