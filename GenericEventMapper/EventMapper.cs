using System.Text;
using GenericMessaging;
using IConsoleLog;
using Newtonsoft.Json.Linq;

namespace GenericEventMapper;

public class EventMapper
{
    private readonly IConsoleLog.IConsoleLog? _log = null;

    public EventMapper(ref Action<SendableTarget>? eventSource, ConsoleLog.ConsoleLog? consoleLog = null)
    {
        _log = consoleLog;
        eventSource += HandleEvent;
    }

    public EventMapper(IConsoleLog.IConsoleLog? log)
    {
        _log = log;
    }

    public void HandleEvent(SendableTarget target)
    {
        _log?.WriteLog(message: "Got Event: " + target.TargetInfo);
        if (handlers.TryGetValue(target.TargetInfo, out var handler))
            handler.HandleEvent(target);
        else
        {
            _log?.WriteLog(message: "No handler for event: " + target.TargetInfo, logLevel: LogLevel.Warning);
            _log?.WriteLog(
                "Event Target Info: " + target.TargetInfo +
                Environment.NewLine + "Event Data: " +
                Environment.NewLine + DeserializeContainedClass(target).ToString(), logLevel: LogLevel.Debug);
        }
    }

    private static JObject DeserializeContainedClass(SendableTarget targetReceived)
    {
        var containedString = Encoding.Unicode.GetString(targetReceived.ContainedClass);
        //TODO: This should be a try catch
        var jObject = JObject.Parse(containedString);
        //Turn the Jobject into the its sendable target
        var target = jObject.ToObject<SendableTarget>();
        //Turn the contained class into a JObject
        return JObject.Parse(
            Encoding.Unicode.GetString(target?.ContainedClass ?? throw new InvalidOperationException()));
    }

    //DO NOT RENAME FROM MapGenericAction - it is used for reflection
    public void MapGenericAction<T>(string name, object? o)
    {
        //Cast o To Action<T>
        //MapAction<T> it
        var action = (Action<T>)o!;
        //Throw if null
        if (action == null)
            throw new Exception("Action is null");

        MapAction<T>(name, action);
    }


    public void MapAction<T>(string eventName, Action<T> action)
    {
        var handler = new EventHandler<T>(action);

        //check if the event is already mapped, if so, remove it from the dictionary and add the new one
        if (handlers.ContainsKey(eventName))
            handlers.Remove(eventName);

        handlers.Add(eventName, handler);
    }

    public void UnregisterAction(string eventName)
    {
        handlers.Remove(eventName);
    }

    private Dictionary<string, IEventHandler> handlers = new();

    #region Event Handler

    interface IEventHandler
    {
        public void HandleEvent(SendableTarget target);
    }

    private class EventHandler<T> : IEventHandler
    {
        public EventHandler(Action<T> @event)
        {
            _event = @event;
        }

        private readonly Action<T> _event;

        private void SendEvent(JObject jObject)
        {
            _event?.Invoke(jObject.ToObject<T>() ?? throw new InvalidOperationException("Could not convert to object"));
        }

        public void HandleEvent(SendableTarget obj)
        {
            SendEvent(DeserializeContainedClass(obj));
        }
    }

    #endregion
}