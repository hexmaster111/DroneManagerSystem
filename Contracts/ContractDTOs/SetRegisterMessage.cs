using GenericMessaging;
using Newtonsoft.Json.Linq;

namespace Contracts.ContractDTOs;

public class SetRegisterMessage : ISendable
{
    public SetRegisterMessage(string registerName, object value)
    {
        RegisterName = registerName;
        Value = value;
    }

    public string RegisterName { get; set; }
    public object Value { get; set; }
    
    public JObject ToJson()
    {
        return JObject.FromObject(this);
    }
}