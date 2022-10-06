using GenericMessaging;
using Newtonsoft.Json.Linq;

namespace Contracts.ContractDTOs;

public class BlankRequest : ISendable
{
    public JObject ToJson()
    {
        return new JObject();
    }
}