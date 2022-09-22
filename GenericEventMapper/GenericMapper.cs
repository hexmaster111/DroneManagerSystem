using System.Text;
using DroneManager.Interface.ServerInterface;
using IConsoleLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GenericEventMapper;

public class EventMapper
{
    private readonly ConsoleLog.ConsoleLog? _log = null;
    
    public EventMapper(ref Action<SendableTarget> eventSource, ConsoleLog.ConsoleLog? consoleLog = null)
    {
        _log = consoleLog;

        eventSource += HandleEvent;
    }
    
    private void HandleEvent(SendableTarget target)
    {
        _log?.WriteLog(message: "Got Event: " + target.TargetInfo);
        if (handlers.TryGetValue(target.TargetInfo, out var handler))
            handler.HandleEvent(target);
        else
            _log.WriteLog(message:"No handler for event: " + target.TargetInfo, logLevel:LogLevel.Warning);
    }

    public void MapAction<T>(Action<T> action, string eventName)
    {
        var handler = new EventHandler<T>(action);
        handlers.Add(eventName, handler);
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