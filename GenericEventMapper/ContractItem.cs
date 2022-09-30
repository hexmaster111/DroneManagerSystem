using GenericMessaging;

namespace GenericEventMapper;

public abstract class ContractItem<T>
{
    public abstract string Name { get; }
    public abstract Action<T> Action { get; set; }

    public abstract void Send(ISendable value);

    public abstract void InitSender(object[] args);

    public Func<bool> RefreshContract;
}