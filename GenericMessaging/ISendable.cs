using Newtonsoft.Json.Linq;

namespace GenericMessaging;

public interface ISendable
{
    public JObject ToJson();
}