using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.Remote;
using DroneManager.Interface.RemoteConnection;
using DroneManager.Interface.RemoteHardware;
using GenericMessaging;

namespace TestAssetImpl;

public static class TestDroneGenerator
{

    public static Drone Generate()
    {
        return new DroneImpl();
    }
    
    private class DroneImpl : Drone
    {
        public Location CurrentLocation { get; }
        public VitalDto VitalsDto => new VitalDtoImpl();
        public DroneControl Control => new ControlImpl();
        public DroneId Id => new DroneId(DroneType.Experimental, 1234);
    }


    private class ControlImpl : DroneControl
    {
        public  Queue<ITask> Tasks
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

        public  ITask Task { get; }

        public override DroneControllableHardware? ControllableHardware => new ControllableHardwareImpl();


        private class ControllableHardwareImpl : DroneControllableHardware
        {
            public override ControllableHardwareMetaData GetHardwareMetaData()
            {
                return new ControllableHardwareMetaData()
                {
                    Name = "Test Hardware",
                    Description = "Testing hardware",
                    Documentation = null,

                };
            }

            public override DroneRemoteRegister[] Registers
            {
                get =>
                    new DroneRemoteRegister[]
                    {
                        new RemoteRegisterImpl(0),
                        new RemoteRegisterImpl(1)
                    };
                init => throw new NotImplementedException();
            }

            private class RemoteRegisterImpl : DroneRemoteRegister
            {
                public RemoteRegisterImpl(int i)
                {
                    Name = $"Register {i}";
                    RegisterDescription = $"This is register {i}";
                    DataType = DataType.Int;
                    Value = i;
                }

                public override string Name { get; }
                public string? RegisterDescription { get; }
                public override DataType DataType { get; }
                public override object Value { get; set; }
            }
        }
    }

    private class VitalDtoImpl : VitalDto
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