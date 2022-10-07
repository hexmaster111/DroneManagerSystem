using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GenericMessaging;

public class SenableDtoBase
{
    public JObject ToJson()
    {
        return JObject.FromObject(this);
    }
}