using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.Remote;

namespace ServerBackend;

public class DroneClient : IDrone
{
    public IRemoteClient? RemoteClient { get; set; }

    public void OnConnect()
    {
        
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
    
    
    public IVital Vitals { get; }
    
    public IControl Control { get; }

    #endregion
}