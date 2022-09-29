using GenericMessaging;

namespace Contracts;

public class ContractItem<T>
{
    public Action<T> Action { get; set; }
    public GenericWriter Writer { get; set; }

    public void InitSender(GenericWriter writer, string name)
    {
        Writer = writer;
        Name = name;
    }

    public string Name;
    
    public void Send(ISendable value)
    {
        if (Writer == null || Name == null)
            throw new Exception("This contract item was not registered with the SendingContractRegister, " +
                                "it is either being misused or not registered.");
        
        Writer.SendData(new SendableTarget(value, Name));
    }
}