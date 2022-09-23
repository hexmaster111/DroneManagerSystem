using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GenericMessaging;

public interface ISendable
{
    public JObject ToJson();
}