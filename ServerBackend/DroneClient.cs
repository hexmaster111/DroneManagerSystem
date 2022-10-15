using Contracts;
using Contracts.ContractDTOs;
using DroneManager.Interface;
using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.Remote;
using DroneManager.Interface.RemoteHardware;

namespace ServerBackend;

public class DroneClient : Drone
{
    public override string ToString()
    {
        return "DroneClient " + this.Id;
    }

    public DroneClient(RemoteClient.RemoteClient? remoteClient) : base()
    {
        RemoteClient = remoteClient;
    }

    public RemoteClient.RemoteClient? RemoteClient { get; set; }
    public Action<DroneClient> OnDisconnect { get; set; }


    public void OnConnect()
    {
        _control = new DroneControlImpl(RemoteClient);

        RemoteClient.ReceivingContract.VitalsUpdate.Action += OnVitalsUpdate;
        RemoteClient.ReceivingContract.LocationUpdate.Action += LocationUpdate;
        RemoteClient.ReceivingContract.HardwareInfoUpdate.Action += HardwareInfoUpdate;
        RemoteClient.ReceivingContract.HeartBeat.Action += HeartBeat;
        


        //Disconnect handler
        RemoteClient.OnConnectionStatusChanged += (status) =>
        {
            if (status == ConnectionStatus.Disconnected)
            {
                OnDisconnect(this);
            }
        };

        RemoteClient.ReceivingContract.RefreshReceivingContract();
    }

    private void HeartBeat(HeartBeatSuperMessage obj)
    {
        OnVitalsUpdate(obj.Vitals);
        CurrentLocation = obj.Location.Location;
       // _control.OnHardwareInfoUpdate(obj.HardwareInfo);
       HardwareInfoUpdate(obj.HardwareInfo);
    }


    #region IDrone Members

    #region Drone Control Implementation
    
    private DroneControlImpl _control;


    private class DroneControlImpl : IDroneControl
    {
        public DroneControlImpl(RemoteClient.RemoteClient remoteClient)
        {
            _controllableHardware = new ControllableHardwareImpl(remoteClient);
        }


        private class ControllableHardwareImpl : DroneControllableHardware
        {
            public ControllableHardwareImpl(RemoteClient.RemoteClient remoteClient)
            {
                _remoteClient = remoteClient;
            }

            private RemoteClient.RemoteClient _remoteClient;

            private Dictionary<string, RegisterImpl> _registers = new();


            public class RegisterImpl : DroneRemoteRegister
            {
                private string _name;
                private DataType _dataType;

                private RemoteClient.RemoteClient _remoteClient;

                public RegisterImpl(string name, DataType dataType, object valueStorage,
                    RemoteClient.RemoteClient remoteClient)
                {
                    _name = name;
                    _dataType = dataType;
                    ValueStorage = valueStorage;
                    _remoteClient = remoteClient;
                }

                public override string Name => _name;
                public override DataType DataType => _dataType;

                public override object Value
                {
                    get => ValueStorage;
                    set
                    {
                        _remoteClient.SendingContract.SetRegister.Send(new SetRegisterMessage(_name, value));
                        ValueStorage = value;
                    }
                }

                public object ValueStorage;
            }

            public void UpdateRegisterValues(HardwareInfoUpdateMessage updateMessage)
            {
                foreach (var register in updateMessage.Data)
                {
                    //Check if the key is in the dict
                    if (_registers.ContainsKey(register.Name))
                    {
                        //Update the value
                        //setting _value directly to avoid sending a message to the drone
                        _registers[register.Name].ValueStorage = register.Value;
                    }
                    else
                    {
                        //Add the register to the dict
                        _registers.Add(register.Name,
                            new RegisterImpl(register.Name, register.DataType, register.Value, _remoteClient));
                    }
                }
            }

            public override ControllableHardwareMetaData GetHardwareMetaData()
            {
                throw new NotImplementedException();
            }

            // public override event DroneRemoteRegister[] OnHardwareRegisterUpdate;


            public override DroneRemoteRegister[] Registers
            {
                get => _registers.Select(register => register.Value)
                    .Cast<DroneRemoteRegister>()
                    .ToArray();
                init => throw new NotImplementedException();
            }
        }

        public DroneControllableHardware? ControllableHardware => _controllableHardware;

        private readonly DroneControllableHardware? _controllableHardware;

        public void OnHardwareInfoUpdate(HardwareInfoUpdateMessage message)
        {
            (_controllableHardware as ControllableHardwareImpl)?.UpdateRegisterValues(message);
        }
    }

    private void HardwareInfoUpdate(HardwareInfoUpdateMessage obj)
    {
        _control?.OnHardwareInfoUpdate(obj);
        Control = _control;
        base.OnRemoteRegisterChanged?.Invoke(obj.Data);
    }

    #endregion

    #region DroneId Implementation

    public void SetDroneId(DroneId id)
    {
        base.Id = id;
        base.OnIdChanged?.Invoke(id);
    }

    #endregion

    #region Vitals Implementation

    public void OnVitalsUpdate(VitalsUpdateMessage vitals)
    {
        Vitals.Temperature = vitals.Temperature;
        Vitals.BreathingRate = vitals.BreathingRate;
        Vitals.HeartRate = vitals.HeartRate;
        base.OnVitalChanged?.Invoke(Vitals);
    }

    #endregion

    #region Location Implementation

    private void LocationUpdate(LocationMessage obj)
    {
        base.CurrentLocation = obj.Location;
        base.OnLocationChanged?.Invoke(obj.Location);
    }

    #endregion

    #endregion
}