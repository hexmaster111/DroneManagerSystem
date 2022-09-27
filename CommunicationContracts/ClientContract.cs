using DroneManager.Interface.ServerInterface;
using GenericEventMapper;
using GenericMessaging;

namespace CommunicationContracts;


public interface IContract
{
    MapActionContract<ISendable>[] ContractItems { get; }
}

public class ClientContract : IContract 
{
    public MapActionContract<HandShakeMessage> HandShake = new("HandShake");

    public MapActionContract<ISendable>[] ContractItems => new MapActionContract<ISendable>[]
    {
        HandShake
    };
}