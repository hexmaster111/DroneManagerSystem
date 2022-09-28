using System.Diagnostics;
using DroneManager.Interface.ServerInterface;
using GenericEventMapper;

namespace Contracts;

public class ServerEndpointContract
{
    public ContractItem<HandShakeMessage> HandShake { get; } = new();
    public ContractItem<HandShakeMessage> HandShake2 { get; } = new();


    // public void Register(ref EventMapper eventMapper)
    // {
    //     eventMapper.MapAction("HandShake" ,HandShake.Action);
    // }
}

public static class ContractRegister
{
    /// <summary>
    ///     Registers all contracts into the event mapper
    /// </summary>
    /// <param name="eventMapper"></param>
    /// <param name="contract"></param>
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

            //Get the Action<T> property
            var actionProperty = contractItem.PropertyType.GetProperty("Action");

            //Get the Action<T> value
            var action = actionProperty.GetValue(contractItem.GetValue(contract));

            if (action == null)
                throw new Exception(
                    $"{contractItemMemberName} was not registered to, Add your method to the contract before registering");

            // eventMapper.MapAction<compilerMadeType>(contractItemMemberName, actionProperty.GetValue(contractItem));

            //Get the Action<T> type
            var actionType = actionProperty.PropertyType;

            //Get the T type
            var genericType = actionType.GetGenericArguments()[0];

            //Get the MapAction method
            var mapActionMethod = eventMapper.GetType()
                .GetMethod("Test")
                ?.MakeGenericMethod(genericType);

            Debug.Assert(mapActionMethod != null, nameof(mapActionMethod) + " != null");


            //Call the MapAction method
            mapActionMethod.Invoke(eventMapper, new object[] { contractItemMemberName, action });


            // eventMapper.MapAction(contractItemMemberName, action);
        }
    }
}