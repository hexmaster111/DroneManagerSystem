using IConsoleLog;

namespace GenericEventMapper;

public static class SendingContractRegister
{
    public static void RegisterSendingContract(object contract, object[] sendSetupArgs, IConsoleLog.IConsoleLog log)
    {
        //Get all ContractItem properties
        var contractItems = contract.GetType()
            .GetProperties()
            .Where(x => x.PropertyType.IsGenericType &&
                        x.PropertyType.GetGenericTypeDefinition() == typeof(ContractItem<>));

        //Run the ContractItem's InitSender method
        foreach (var contractItem in contractItems)
        {
            var contractItemValue = contractItem.GetValue(contract);
            var contractItemInitSenderMethod = contractItemValue?.GetType().GetMethod("InitSender");
            if (contractItemInitSenderMethod == null)
            {
                log.WriteLog($"{contractItem.Name} does not have an InitSender method", LogLevel.Error);
                continue;
            }

            //put the name as the first sendSetupArgs
            var sendSetupArgsWithContractName = new object[sendSetupArgs.Length + 1];
            sendSetupArgsWithContractName[0] = contractItem.Name;
            Array.Copy(sendSetupArgs, 0, sendSetupArgsWithContractName, 1, sendSetupArgs.Length);


            contractItemInitSenderMethod.Invoke(contractItemValue, new object?[] { sendSetupArgsWithContractName });
        }
    }
}