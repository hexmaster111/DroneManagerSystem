using System.Runtime.CompilerServices;

namespace IConsoleLogInterface;

public enum LogLevel
{
    Info,
    Warning,
    Error,
    Debug,
    Fatal,
    Notice
}


public interface IConsoleLog
{
    public void WriteLog([CallerMemberName]string caller = "" , string message = "", LogLevel logLevel = LogLevel.Info);
    public void WriteCommandLog(string command, string message = "", LogLevel logLevel = LogLevel.Info);
}