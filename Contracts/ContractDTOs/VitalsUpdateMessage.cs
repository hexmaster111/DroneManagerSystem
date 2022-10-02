using GenericMessaging;
using Newtonsoft.Json.Linq;

namespace Contracts.ContractDTOs;

public class VitalsUpdateMessage : ISendable
{
    
    
    
    
    public JObject ToJson()
    {
        return JObject.FromObject(this);
    }
}