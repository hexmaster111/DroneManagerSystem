using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GenericMessaging;

public class GenericReader
{
    private readonly NetworkStream  _stream;

    public GenericReader(NetworkStream stream)
    {
        _stream = stream;
    }

    public SendableTarget? ReadData()
    {
        if (!_stream.DataAvailable)
            return null;
        
        var buffer = new byte[1024];
        var read = _stream.Read(buffer, 0, buffer.Length);
        var data = Encoding.ASCII.GetString(buffer, 0, read);
        var message = JObject.Parse(data);
        var target = message.ToObject<SendableTarget>();
        if(target?.TargetInfo == null) throw new ("Target Null Exception");
        return target;

    }
}