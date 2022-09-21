using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DroneManager.Interface.ServerInterface;

/// <summary>
/// Blank interface to make all messages seasonable
/// </summary>
public interface ISendable
{
    public JObject ToJson();
}

public class SendableTarget : ISendable
{
    
    public SendableTarget(string target, JObject jObj)
    {
        TargetInfo = target;
        containedClass = Encoding.Unicode.GetBytes(jObj.ToString());
    }
    
    [JsonConstructor]
    public SendableTarget(string targetInfo, byte[] containedClass)
    {
        TargetInfo = targetInfo;
        this.containedClass = containedClass;
    }

    public string TargetInfo { get; }

    public byte[] containedClass { get; }

    private bool _serialized = false;


    public JObject ToJson()
    {
        return JObject.FromObject(this);
    }
}