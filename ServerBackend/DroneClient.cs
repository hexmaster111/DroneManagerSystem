using Contracts;
using Contracts.ContractDTOs;
using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.Remote;
using DroneManager.Interface.RemoteHardware;

namespace ServerBackend;

public class DroneClient : IDrone
{
    public DroneClient(RemoteClient.RemoteClient? remoteClient)
    {
        RemoteClient = remoteClient;
    }

    public RemoteClient.RemoteClient? RemoteClient { get; set; }
    public Action<DroneClient> OnDisconnect { get; set; }


    public void OnConnect()
    {
        Vitals = new VitalImpl(RemoteClient.ReceivingContract);
        _control = new DroneControlImpl(RemoteClient);

        RemoteClient.ReceivingContract.LocationUpdate.Action += LocationUpdate;
        RemoteClient.ReceivingContract.HardwareInfoUpdate.Action += HardwareInfoUpdate;


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


    #region IDrone Members

    #region Drone Control Implementation

    public DroneControl Control => _control ?? throw new Exception("Drone is not connected");
    private DroneControlImpl _control;

    private class DroneControlImpl : DroneControl
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
                
                public RegisterImpl(string name, DataType dataType, object value, RemoteClient.RemoteClient remoteClient)
                {
                    _name = name;
                    _dataType = dataType;
                    _value = value;
                    _remoteClient = remoteClient;
                }

                public override string Name => _name;
                public override DataType DataType => _dataType;

                public override object Value
                {
                    get => _value;
                    set
                    {
                        _remoteClient.SendingContract.SetRegister.Send(new SetRegisterMessage(_name, value));
                        _value = value;
                    }
                }

                public object _value;
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
                        _registers[register.Name]._value = register.Value;
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

            public override DroneRemoteRegister[] Registers
            {
                get => _registers.Select(register => register.Value)
                    .Cast<DroneRemoteRegister>()
                    .ToArray();
                init => throw new NotImplementedException();
            }
        }

        public override DroneControllableHardware ControllableHardware => _controllableHardware;

        private readonly DroneControllableHardware _controllableHardware;

        public void OnHardwareInfoUpdate(HardwareInfoUpdateMessage message)
        {
            (_controllableHardware as ControllableHardwareImpl)?.UpdateRegisterValues(message);
        }
    }

    private void HardwareInfoUpdate(HardwareInfoUpdateMessage obj)
    {
        _control?.OnHardwareInfoUpdate(obj);
    }

    #endregion


    #region DroneId Implementation

    /// <summary>
    ///     The DroneID is a unique identifier for the drone. It is used to identify the drone in the network.
    ///     This value is set when the drone is registered at the server.
    /// </summary>
    public DroneId Id { get; private set; }

    public void SetDroneId(DroneId id)
    {
        Id = id;
    }

    #endregion

    #region Vitals Implementation

    public IVital Vitals { get; private set; }

    private class VitalImpl : IVital
    {
        public VitalImpl(ServerEndpointContract serverEndpointContract)
        {
            serverEndpointContract.VitalsUpdate.Action += VitalsUpdate_Action;
            serverEndpointContract.RefreshReceivingContract();
        }

        private void VitalsUpdate_Action(VitalsUpdateMessage obj)
        {
            Temperature = obj.Temperature;
            HeartRate = obj.HeartRate;
            BreathingRate = obj.BreathingRate;
        }

        public double Temperature { get; private set; } = double.NaN;
        public double HeartRate { get; private set; } = double.NaN;
        public double BreathingRate { get; private set; } = double.NaN;
        public double MaxTemperature => 40;
        public double MaxHeartRate => 200;
        public double MaxBreathingRate => 40;
        public double MinTemperature => 20;
        public double MinHeartRate => 40;
        public double MinBreathingRate => 10;
    }

    #endregion

    #region Location Implementation

    public Location CurrentLocation { get; private set; }


    private void LocationUpdate(LocationMessage obj)
    {
        CurrentLocation = obj.Location;
    }

    #endregion

    #endregion
}