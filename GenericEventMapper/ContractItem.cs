using GenericMessaging;

namespace GenericEventMapper;

public abstract class ContractItem<T>
{
    public abstract string Name { get; }
    public abstract Action<T> Action { get; set; }

    public abstract void Send(SenableDtoBase value);

    public abstract void InitSender(object[] args);
}