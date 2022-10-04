using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.Remote;
using DroneManager.Interface.RemoteConnection;
using DroneManager.Interface.RemoteHardware;
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
        public Location CurrentLocation { get; }
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
                    Name = $"Register {i}";
                    RegisterDescription = $"This is register {i}";
                    DataType = DataType.I64;
                    Value = i;
                }

                public string Name { get; }
                public string? RegisterDescription { get; }
                public DataType DataType { get; }
                public object Value { get; set; }
            }
        }
    }

    private class VitalImpl : IVital
    {
        public double Temperature => 98.6;
        public double HeartRate => 60;
        public double BreathingRate => 12;
        public double MaxTemperature => 100;
        public double MaxHeartRate => 100;
        public double MaxBreathingRate => 20;
        public double MinTemperature => 95;
        public double MinHeartRate => 50;
        public double MinBreathingRate => 10;
    }
    
}