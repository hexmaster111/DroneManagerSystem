using System.Text;
using Newtonsoft.Json;

namespace GenericMessaging;

public class GenericWriter
{
    private readonly TapSynchronized<Stream> _synchronizedStream;

    public Action StreamClosed;

    public GenericWriter(Stream stream)
    {
        _synchronizedStream = new TapSynchronized<Stream>(stream);
    }

    public void SendData(SendableTarget data)
    {
        var target = new SendableTarget(data.TargetInfo, data.ToJson());

        var send = target.ToJson();

        var buffer = Encoding.ASCII.GetBytes(send.ToString(Formatting.None));

        try
        {
            _synchronizedStream.WithValue<object>((ref Stream stream) =>
            {
                stream.Write(buffer, 0, buffer.Length);
                return null;
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            StreamClosed?.Invoke();
            throw;
        }
    }
}