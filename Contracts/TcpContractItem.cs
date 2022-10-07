using GenericEventMapper;
using GenericMessaging;

namespace Contracts;

public class TcpContractItem<T> : ContractItem<T>
{
    public override void Send(SenableDtoBase value)
    {
        if (Writer == null || Name == null)
            throw new Exception("This contract item was not registered with the SendingContractRegister, " +
                                "it is either being misused or not registered.");

        try
        {
            Writer.SendData(new SendableTarget(value, Name));
        }
        catch (Exception e)
        {
            StreamClosed?.Invoke();
        }
    }

    public Action StreamClosed { get; set; }
    
    private GenericWriter Writer { get; set; }

    public override void InitSender(object[] args)
    {
        //GenericWriter writer, string name
        _name = args[0] as string ?? throw new InvalidOperationException();
        Writer = args[1] as GenericWriter ?? throw new InvalidOperationException();
    }

    private string _name;

    public override string Name => _name;

    public override Action<T> Action { get; set; }
}