using CrappyLicenseTool;
using DroneManager.Interface.ServerInterface;

namespace Contracts;

public class ServerEndpointContract
{
    public ContractItem<HandShakeMessage> HandShake { get; } = new();
    public ContractItem<HandShakeMessage> HandShake2 { get; } = new();

}