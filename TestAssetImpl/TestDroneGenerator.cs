using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.Remote;
using DroneManager.Interface.RemoteConnection;
using DroneManager.Interface.RemoteHardware;
using DroneManager.Interface.ServerInterface;
using GenericMessaging;

namespace TestAssetImpl;

public static class TestDroneGenerator
{

    public static IDrone Generate()
    {
        return new DroneImpl();
    }
    
    private class DroneImpl : IDrone
    {
        public IRemoteStreamConnection Connection => new RemoteConnectionImpl();

        public Location CurrentLocation => new Location()
        {
            Latitude = 0,
            Longitude = 0,
            Speed = 0,
            TimeStamp = DateTime.Now,
            LocationProvider = "Debugging",
            LocationName = "The Lab",
            LocationAddress = "The Hive"
        };

        public IVital Vitals => new VitalImpl();
        public IControl Control => new ControlImpl();
        public DroneId Id => new DroneId(DroneType.Experimental, 1234);
    }


    private class ControlImpl : IControl
    {
        public Queue<ITask> Tasks
        {
            get
            {
                var tasks = new Queue<ITask>();
                for (int i = 0; i < 5; i++)
                {
                    tasks.Enqueue(new TestTaskGenerator(i));
                }

                return tasks;
            }
        }

        public ControlMode Mode { get; set; } = ControlMode.Auto;

        public IControllableHardware?[]? ControllableHardware => new IControllableHardware?[]
            { new ControllableHardwareImpl() };


        private class ControllableHardwareImpl : IControllableHardware
        {
            public ControllableHardwareMetaData GetHardwareMetaData()
            {
                return new ControllableHardwareMetaData()
                {
                    HardwareVersion = "1.0",
                    Name = "Test Hardware",
                    Description = "Testing hardware",
                    Manufacturer = "Test Inc.",
                    Model = "Test Model",
                    SerialNumber = Guid.Empty,
                    Documentation = null,
                    FirmwareVersion = "1.0",
                    SoftwareVersion = "1.0",
                };
            }

            public IRemoteRegister[] Registers => new IRemoteRegister[]
            {
                new RemoteRegisterImpl(0),
                new RemoteRegisterImpl(1)
            };

            private class RemoteRegisterImpl : IRemoteRegister
            {
                public RemoteRegisterImpl(int i)
                {
                    RegisterName = $"Register {i}";
                    RegisterDescription = $"This is register {i}";
                    RegisterDataType = typeof(int);
                    RegisterValue = i;
                }

                public string RegisterName { get; }
                public string? RegisterDescription { get; }
                public Type RegisterDataType { get; }
                public object RegisterValue { get; set; }
            }
        }
    }

    private class VitalImpl : IVital
    {
        public double Temperature => 98.6;
        public double HeartRate => 60;
        public double BreathingRate => 12;
    }

    private class RemoteConnectionImpl : IRemoteStreamConnection
    {
        public event Action<object>? DataSent;
        public event Action<ISendable> ConnectionStatusChanged;
        public ConnectionType ConnectionType => ConnectionType.Debugging;
        public ConnectionStatus Status => ConnectionStatus.NotTried;

        public void Disconnect(ISendable? disconnectionArgs)
        {
            throw new NotImplementedException();
        }

        public void Connect(object? connectionArgs)
        {
            throw new NotImplementedException();
        }

        public void SendData(ISendable data)
        {
            throw new NotImplementedException();
        }

        public event Action<ISendable> DataReceived;

        public void Disconnect(object? disconnectionArgs)
        {
            throw new NotImplementedException();
        }

        public void SendData(object data)
        {
            throw new NotImplementedException();
        }

        public bool RequestData(object requestArgs)
        {
            throw new NotImplementedException();
        }

        public IRemoteStreamConnection Connection { get; }
    }
}