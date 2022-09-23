﻿using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.ServerInterface;
using GenericEventMapper;
using GenericMessaging;

public static class ActionMapperTests
{
    public static Action<SendableTarget> testSource;
    public static ConsoleLogging.ConsoleLog Log = new ConsoleLogging.ConsoleLog();
    private static bool _run = true;

    private static void Main(string[] args)
    {
        Log.StartLogWriter();

        EventMapper mapper = new EventMapper(ref testSource, Log);

        mapper.MapAction("DEBUG", new Action<HandShakeMessage>(Action));
        
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