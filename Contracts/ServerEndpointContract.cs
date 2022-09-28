using DroneManager.Interface.ServerInterface;
using GenericEventMapper;

namespace Contracts;

public class ContractItem<T>
{
    public Action<T> Action { get; set; }
}

public class ServerEndpointContract
{
    public ContractItem<HandShakeMessage> HandShake { get; }
    public ContractItem<HandShakeMessage> HandShake2 { get; }
    
    
    // public void Register(ref EventMapper eventMapper)
    // {
    //     eventMapper.MapAction("HandShake" ,HandShake.Action);
    // }
}

public static class ContractRegister
{
    public static void RegisterContracts(ref EventMapper eventMapper, object contract)
    {
        //Get all ContractItem properties
        var contractItems = contract.GetType()
            .GetProperties()
            .Where(x => x.PropertyType.IsGenericType &&
                        x.PropertyType.GetGenericTypeDefinition() == typeof(ContractItem<>));
        
        
        //Register all ContractItems
        foreach (var contractItem in contractItems)
        {
            var contractItemMemberName = contractItem.Name;
            
            // Pass the action<T> to the eventMapper
            var action = (Delegate) contractItem.GetValue(contract);

            // eventMapper.MapAction(contractItemMemberName, action);
        }
    }
}