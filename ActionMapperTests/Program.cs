using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.ServerInterface;
using GenericEventMapper;

public static class ActionMapperTests
{
    public static Action<SendableTarget> testSource;
    public static ConsoleLog.ConsoleLog Log = new ConsoleLog.ConsoleLog();
    private static bool _run = true;

    private static void Main(string[] args)
    {
        Log.StartLogWriter();

        EventMapper mapper = new EventMapper(ref testSource, Log);

        mapper.MapAction(new Action<HandShakeMessage>(Action), "DEBUG");
        
        int i = 0;
        
        while (_run)
        {
            Console.ReadLine();
            testSource?.Invoke(new SendableTarget("DEBUG",
                new HandShakeMessage(
                        new DroneId(
                            DroneType.Experimental,
                            5050 + i++))
                    .ToJson()));
        }
    }

    private static void Action(HandShakeMessage obj)
    {
        Log.WriteLog(message:"Debug action called" + obj.Id + " " + obj.TimeStamp);
    }
}