using Contracts.ContractDTOs;
using DroneManager.Interface.GenericTypes;
using GenericEventMapper;
using GenericMessaging;

namespace ActionMapperTests;

public static class ActionMapperTests
{
    public static Action<SendableTarget>? TestSource;
    public static ConsoleLog.ConsoleLog Log = new ConsoleLog.ConsoleLog();
    private static bool _run = true;

    private static void Main(string[] args)
    {
        Log.StartLogWriter();

        EventMapper mapper = new EventMapper(ref TestSource, Log);

        mapper.MapAction("DEBUG", new Action<HandShakeMessage>(Action));
        
        int i = 0;
        
        
        while (_run)
        {
            Console.ReadLine();
            TestSource?.Invoke(new SendableTarget("DEBUG",
                new HandShakeMessage(
                        new DroneId(
                            DroneType.Experimental,
                            5050 + i++))
                    .ToJson()));
        }
    }

    private static void Action(HandShakeMessage obj)
    {
        Log.WriteLog(message: "Debug action called" + obj.Id + " " + obj.TimeStamp);
    }
}