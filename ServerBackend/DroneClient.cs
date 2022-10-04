using Contracts;
using Contracts.ContractDTOs;
using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.Remote;

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

        RemoteClient.ReceivingContract.LocationUpdate.Action += LocationUpdate;


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

    public DroneControl Control { get; }

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