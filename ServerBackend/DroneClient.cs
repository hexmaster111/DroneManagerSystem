using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.Remote;

namespace ServerBackend;

public class DroneClient : IDrone
{
    public IRemoteClient? RemoteClient { get; set; }

    public void OnConnect()
    {
        Vitals = new VitalImpl();
    }

    #region IDrone Members

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

    public Location CurrentLocation { get; }

    public IControl Control { get; }

    #region Vitals Implementation

    public IVital Vitals { get; private set; }

    private class VitalImpl : IVital
    {
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

    #endregion
}