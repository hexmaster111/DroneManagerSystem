using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GenericMessaging;

public class SendableTarget : SenableDtoBase
{
    
    public SendableTarget(SenableDtoBase sendable, string target)
    {
        TargetInfo = target;
        ContainedClass = Encoding.UTF8.GetBytes(sendable.ToJson().ToString());
    }
    
    
    public SendableTarget(string target, JObject jObj)
    {
        TargetInfo = target;
        ContainedClass = Encoding.UTF8.GetBytes(jObj.ToString(Formatting.None));
    }
    
    [JsonConstructor]
    public SendableTarget(string targetInfo, byte[] containedClass)
    {
        TargetInfo = targetInfo;
        this.ContainedClass = containedClass;
    }

    public string TargetInfo { get; }

    public byte[] ContainedClass { get; }

    private bool _serialized = false;


    public JObject ToJson()
    {
        return JObject.FromObject(this);
    }
}