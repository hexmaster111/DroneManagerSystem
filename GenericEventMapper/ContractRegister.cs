using System.Diagnostics;
using CrappyLicenseTool;
using GenericEventMapper;

namespace Contracts;

public class ContractItem<T>
{
    public Action<T> Action { get; set; }
}

/* Example usage */
/*
 *   public class ServerEndpointContract
 *   {
 *       public ContractItem<ISendable> HandShake { get; } = new(); //The new here is IMPORTANT
 *       public ContractItem<ISendable> OtherMessage { get; } = new();
 *   
 *   }
 *
 *  The EventName is generated off the the property name, and the type of the action it will send is the <T> type
 */

public static class ContractRegister
{

    
    /// <summary>
    ///     Registers all contracts into the event mapper
    /// </summary>
    /// <param name="eventMapper"></param>
    /// <param name="contract"></param>
    public static void RegisterContracts(ref EventMapper eventMapper, object contract)
    {
        if(!LicManager.IsValid()) throw new Exception("License is invalid");

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

            //Get the Action<T> type
            var actionType = actionProperty.PropertyType;

            //Get the T type
            var genericType = actionType.GetGenericArguments()[0];

            //Get the MapAction method
            var mapActionMethod = eventMapper.GetType()
                .GetMethod("MapGenericAction")
                ?.MakeGenericMethod(genericType);

            Debug.Assert(mapActionMethod != null, nameof(mapActionMethod) + " != null");


            //Call the MapAction method
            mapActionMethod.Invoke(eventMapper, new object[] { contractItemMemberName, action });
        }
    }
}