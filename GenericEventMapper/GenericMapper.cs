using System.Text;
using ConsoleLogging;
using DroneManager.Interface.ServerInterface;
using GenericMessaging;
using IConsoleLogInterface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GenericEventMapper;

public class EventMapper
{
    private readonly IConsoleLog? _log = null;

    public EventMapper(ref Action<SendableTarget> eventSource, ConsoleLogging.ConsoleLog? consoleLog = null)
    {
        _log = consoleLog;
        eventSource += HandleEvent;
    }

    public EventMapper(IConsoleLog? log)
    {
        _log = log;
    }

    public void HandleEvent(SendableTarget target)
    {
        _log?.WriteLog(message: "Got Event: " + target.TargetInfo);
        if (handlers.TryGetValue(target.TargetInfo, out var handler))
            handler.HandleEvent(target);
        else
            _log.WriteLog(message: "No handler for event: " + target.TargetInfo, logLevel: LogLevel.Warning);
    }

    public void MapAction<T>(string eventName, Action<T> action)
    {
        var handler = new EventHandler<T>(action);
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
            SendEvent(JObject.Parse(Encoding.Unicode.GetString(obj.ContainedClass)));
        }
    }

    #endregion
}