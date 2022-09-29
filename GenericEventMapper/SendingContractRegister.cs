using System.Reflection;
using Contracts;
using GenericMessaging;

namespace GenericEventMapper;

public static class SendingContractRegister
{
    public static void RegisterSendingContract(ref GenericWriter writer, object contract, IConsoleLog.IConsoleLog log)
    {
        //Get all ContractItem properties
        var contractItems = contract.GetType()
            .GetProperties()
            .Where(x => x.PropertyType.IsGenericType &&
                        x.PropertyType.GetGenericTypeDefinition() == typeof(ContractItem<>));
        
        
        //Inject the writer dependency into the ContractItem
        foreach (var contractItem in contractItems)
        {
            var contractItemValue = contractItem.GetValue(contract);
            
            //Find the contractItems InitSender method
            var initSenderMethod = contractItemValue.GetType()
                .GetMethod("InitSender");
            
            //Invoke the method
            initSenderMethod.Invoke(contractItemValue, new object[] {writer, contractItem.Name});
        }
    }
}