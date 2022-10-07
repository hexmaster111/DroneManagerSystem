using GenericMessaging;
using Newtonsoft.Json.Linq;

namespace Contracts.ContractDTOs;

public class ChatMessage : SenableDtoBase
{
    public ChatMessage(string sender, string message)
    {
        Message = message;
        Sender = sender;
    }

    public string Message { get; set; }
    public string Sender { get; set; }
    
}