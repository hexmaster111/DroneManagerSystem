namespace Contracts;

public class ContractItem<T>
{
    public Action<T> Action { get; set; }
}