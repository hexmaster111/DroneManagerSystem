namespace DroneManager.Interface.RemoteConnection;

public enum ConnectionType
{
    Debugging, // Localhost TCP connection
    Serial, //Local, direct connection, mainly for testing (i hope)
    HiveNet, //Internal to the hive (ESP to ... )
    Internet, //External to the hive (3G, 4G, WiFi, etc)
}