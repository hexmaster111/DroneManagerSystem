using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.Remote;
using DroneManager.Interface.RemoteHardware;
using GenericMessaging;
using Newtonsoft.Json.Linq;

namespace Contracts.ContractDTOs;

public class ControllableUpdateMessage : ISendable
{
    
    public IControl ControlInfo { get; set; }
    
    public JObject ToJson()
    {
        return JObject.FromObject(this);
    }
}


public class ControlDtoImpl : IControl
{
    public ControlDtoImpl(Queue<ITask> tasks, ControlMode mode, IControllableHardware?[]? controllableHardware)
    {
        Tasks = tasks;
        Mode = mode;
        ControllableHardware = controllableHardware;
    }

    public Queue<ITask> Tasks { get; }
    public ControlMode Mode { get; set; }
    public IControllableHardware?[]? ControllableHardware { get; }
}


public class ControllableHardwareDtoImpl : IControllableHardware
{
    public ControllableHardwareMetaData GetHardwareMetaData()
    {
        throw new NotImplementedException("This object is only for passing messages");
    }

    public IRemoteRegister[] Registers { get; }
}


public class RemoteRegisterImpl : IRemoteRegister
{
    public RemoteRegisterImpl(string registerName, string? registerDescription, DataType registerDataType, object registerValue)
    {
        Name = registerName;
        RegisterDescription = registerDescription;
        DataType = registerDataType;
        Value = registerValue;
    }

    public string Name { get; }
    public string? RegisterDescription { get; }
    public DataType DataType { get; }
    public object Value { get; set; }
}