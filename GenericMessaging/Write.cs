using System.Text;

namespace GenericMessaging;

public class GenericWriter
{
    private readonly Stream _stream;


    public GenericWriter(Stream stream)
    {
        _stream = stream;
    }

    public void SendData(SendableTarget data)
    {
        var target = new SendableTarget("debug", data.ToJson());

        var send = target.ToJson();

        var buffer = Encoding.ASCII.GetBytes(send.ToString());

        _stream.Write(buffer, 0, buffer.Length);
    }
}