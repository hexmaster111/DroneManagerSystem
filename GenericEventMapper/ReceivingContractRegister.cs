using IConsoleLog;

namespace GenericEventMapper;

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

/// <summary>
///     This is so that a mapper can be automatically assigned to a contract that will be used as a message receiver
///     Use case: A server has a contract that the Client can send to, the server would then use ReceivingContractRegister
/// </summary>
public static class ReceivingContractRegister
{
    /// <summary>
    ///     Registers all contracts into the event mapper
    /// </summary>
    /// <param name="eventMapper"></param>
    /// <param name="contract"></param>
    public static void RegisterContracts(ref EventMapper eventMapper, object contract, Func<bool> refreshCommand,
        IConsoleLog.IConsoleLog log)
    {
        // if (!LicManager.IsValid()) throw new Exception("License is invalid");

        //Get all ContractItem properties
        var contractItems = contract.GetType()
            .GetProperties()
            .Where(x => x.PropertyType.IsGenericType &&
                        x.PropertyType.GetGenericTypeDefinition() == typeof(ContractItem<>));


        //Register all ContractItems
        foreach (var contractItem in contractItems)
        {
            //Get the RefreshContract Func<bool> property, and set it to the refreshCommand
            var refreshContract = contractItem.PropertyType.GetProperty("RefreshContract");
            refreshContract.SetValue(contractItem.GetValue(contract), refreshCommand);


            var contractItemMemberName = contractItem.Name;

            //Get the Action<T> property
            var actionProperty = contractItem.PropertyType.GetProperty("Action");

            if (actionProperty == null)
            {
                log.WriteLog("ContractItem property does not have an Action property", LogLevel.Error);
                continue;
                //Left in code for debugging
                throw new Exception(
                    $"{contractItemMemberName} does not have a property named Action, which is required for ContractItem<T>");
            }

            //Get the Action<T> value
            var action = actionProperty.GetValue(contractItem.GetValue(contract));

            if (action == null)
            {
                log.WriteLog(
                    "{contractItemMemberName} was not registered to, Add your method to the contract before registering",
                    LogLevel.Error);
                continue;
                throw new Exception(
                    $"{contractItemMemberName} was not registered to, Add your method to the contract before registering");
            }

            //Get the Action<T> type
            var actionType = actionProperty.PropertyType;

            //Get the T type
            var genericType = actionType.GetGenericArguments()[0];

            //Get the MapAction method
            var mapActionMethod = eventMapper.GetType()
                .GetMethod("MapGenericAction")
                ?.MakeGenericMethod(genericType);


            //Call the MapAction method
            if (mapActionMethod != null)
                mapActionMethod.Invoke(eventMapper, new object[] { contractItemMemberName, action });
            else
            {
                log.WriteLog("MapAction method is null", LogLevel.Error);
                continue;
                throw new Exception("MapAction method is null");
            }
        }
    }
}