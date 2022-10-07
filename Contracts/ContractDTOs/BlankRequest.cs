using GenericMessaging;
using Newtonsoft.Json.Linq;

namespace Contracts.ContractDTOs;

public class BlankRequest : SenableDtoBase
{
    public new JObject ToJson()
    {
        return new JObject();
    }
}