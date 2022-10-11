using Contracts.ContractDTOs;
using DroneManager.Interface.RemoteHardware;

namespace RegisterSimulator;


public class RegSimulator : DroneControllableHardware
{
    public RegSimulator(int registerCount)
    {
        var a = new List<DroneRemoteRegister>();
        
        for (int i = 0; i < registerCount; i++)
            a.Add(new HardwareInfoUpdateMessage.RemoteRegisterData($"REG{i}", DataType.Int, i));
        
        Registers = a.ToArray();
    }

    public override ControllableHardwareMetaData GetHardwareMetaData()
    {
        return new ControllableHardwareMetaData()
        {
            Description = "Register Simulator",
            Name = "Register Simulator",
            Documentation = null
        };
    }


    public void SetRegisterValue(string register, object value)
    {
        //look for register
        var reg = Registers.FirstOrDefault(x => x.Name == register);
        if (reg == null)
        {
            //TODO: Send error message back to the server
            
            throw new Exception("Register not found");
            return;
        }
        
        //set value
        reg.Value = value;
    }

    public override DroneRemoteRegister[] Registers { get; init; }
}

