using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using IConsoleLog;

namespace ConsoleLog;

public class ConsoleLog : IConsoleLog.IConsoleLog
{
    public static string NameOfCallingClass()
    {
        string fullName;
        Type declaringType;
        int skipFrames = 2;
        do
        {
            MethodBase method = new StackFrame(skipFrames, false).GetMethod();
            declaringType = method.DeclaringType;
            if (declaringType == null)
            {
                return method.Name;
            }

            skipFrames++;
            fullName = declaringType.FullName;
        } while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

        return fullName;
    }

    private static bool _logWriterStarted = false;

    public void StartLogWriter()
    {
        if (_logWriterStarted) return;
        _logWriterStarted = true;
        var logWriter = new Thread(new ThreadStart(LogWriter));
        logWriter.Start();
    }

    private void LogWriter()
    {
        while (true)
        {
            Thread.Sleep(1);
            if (_logQueue.Count <= 0) continue;
            if (!_logQueue.TryDequeue(out var log)) continue;
            Console.ForegroundColor = _logToColor(log.LogLevel);
            Console.WriteLine(log.Message);
            Console.ResetColor();
        }
    }


    private ConcurrentQueue<LogMessage> _logQueue = new();


    private ConsoleColor _logToColor(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Info => ConsoleColor.White,
            LogLevel.Warning => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.Red,
            LogLevel.Debug => ConsoleColor.DarkGray,
            LogLevel.Fatal => ConsoleColor.Blue,
            LogLevel.Notice => ConsoleColor.Green,
            _ => ConsoleColor.DarkGreen
        };
    }


    private class LogMessage
    {
        public string Message { get; set; }
        public LogLevel LogLevel { get; set; }
    }

    public void WriteLog(string message = "", LogLevel logLevel = LogLevel.Info, [CallerMemberName] string caller = "")
    {
        var finalMessage = $"[{DateTime.Now:hh:mm:ss.fff}][{logLevel}][{NameOfCallingClass()}.{caller}] {message}";
        if (_logWriterStarted)
        {
            _logQueue.Enqueue(new LogMessage { Message = finalMessage, LogLevel = logLevel });
            return;
        }

        //Set the console color to the log level color.
        Console.ForegroundColor = _logToColor(logLevel);
        Console.WriteLine(finalMessage);
        Console.ForegroundColor = ConsoleColor.White;
    }
    

    public void WriteCommandLog(string command, string message = "", LogLevel logLevel = LogLevel.Info)
    {
        var finalMessage = $"[{logLevel}][{command}] {message}";
        if (_logWriterStarted)
        {
            _logQueue.Enqueue(new LogMessage { Message = finalMessage, LogLevel = logLevel });
            return;
        }

        //Set the console color to the log level color.
        Console.ForegroundColor = _logToColor(logLevel);
    }
}