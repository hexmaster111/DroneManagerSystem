using Contracts.ContractDTOs;
using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.RemoteHardware;
using TestAssetImpl;
using TestDroneNetworkImpl;

namespace ControlBuilderTest;

public class ControllableMessageTests
{
    [SetUp]
    public void Setup()
    {
    }


    [Test]
    public void SerialiseDeserializeTest()
    {
        ControllableUpdateMessage msg = new ControllableUpdateMessage();
        msg.ControlInfo = new ControlDtoImpl(new Queue<ITask>(new[]
            {
                new TestTaskGenerator(1)
            }), ControlMode.Default, new IControllableHardware[]
            {
                new ControllableHardwareDtoImpl(),
                new ControllableHardwareDtoImpl()
            }
        );
        
        
        
        var serialised = msg.ToJson();

        var deserialised = serialised.ToObject(typeof(ControllableUpdateMessage));
        
        Assert.AreEqual(msg, deserialised);
    }
}