using DroneManager.Interface.RemoteHardware;
using GenericMessaging;

namespace Contracts.ContractDTOs;

public class HardwareInfoUpdateMessage : SenableDtoBase
{
    #region Single Device

    public class RemoteRegisterData : DroneRemoteRegister
    {
        public RemoteRegisterData(string name, DataType dataType, object value)
        {
            Name = name;
            DataType = dataType;
            Value = value;
        }

        public override string Name { get; }
        public override DataType DataType { get; }
        public override object Value { get; set; }
    }

    public HardwareInfoUpdateMessage(RemoteRegisterData[] data)
    {
        Data = data;
    }

    #endregion

    public DroneRemoteRegister[] Data { get; set; }



}