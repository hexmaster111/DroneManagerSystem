
/*
using System;
using System.Collections.Generic;
using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.Remote;
using DroneManager.Interface.RemoteHardware;
using GraphicalConsole.BaseUcs;

namespace GraphicalConsole;

public class ViewTests
{

#region Location View Test

if (false)
{
    var testLocationView = new LocationView();
    var location = new Location
    {
        Latitude = 10,
        Longitude = 20,
        LocationAddress = "666 Sixth Street",
        LocationName = "Test Location"
    };

    testLocationView.Location = location;
    CcTest.Content = testLocationView;
}

#endregion

#region Vital View Test

if (false)
{
    var testVitalView = new VitalView();
    var vital = new VitalDto
    {
        Temperature = 420,
        HeartRate = 69,
        BreathingRate = 42
    };

    testVitalView.Vital = vital;
    CcTest.Content = testVitalView;
}

#endregion

#region DroneId View Test

if (false)
{
    var droneIdView = new DroneIdView();
    var droneId = new DroneId(DroneType.Experimental, 5050);
    droneIdView.DroneId = droneId;

    //CcTest.Content = droneIdView;
}

#endregion

#region Remote Hardware View Test

if (false)
{
    var controlView = new DroneRegisterView();

    var remoteRegisterData = new List<RemoteRegisterData>();

    for (var i = 0; i < 15; i++)
    {
        remoteRegisterData.Add(new RemoteRegisterData($"REG{i}", DataType.Int, 0));
    }


    var remoteHardware = new ControllableHardware(remoteRegisterData.ToArray());

    controlView.ControllableHardware = remoteHardware;

    CcTest.Content = controlView;
}

#endregion


}

public class ControllableHardware : DroneControllableHardware
{
    public ControllableHardware(RemoteRegisterData[] registers)
    {
        Registers = registers;
    }

    public override ControllableHardwareMetaData GetHardwareMetaData()
    {
        throw new NotImplementedException();
    }

    public override DroneRemoteRegister[] Registers { get; init; }
}

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

*/