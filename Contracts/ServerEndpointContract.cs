using CrappyLicenseTool;
using DroneManager.Interface.ServerInterface;

namespace Contracts;

public class ServerEndpointContract //Things the server can receive and the client can send
{
    public ContractItem<HandShakeMessage> HandShake { get; } = new();
    public ContractItem<HandShakeMessage> HandShake2 { get; } = new();

}

public class ClientEndpointContract //Things the client can receive and the server can send
{
    public ContractItem<HandShakeMessage> HandShake { get; } = new();
}