using System.Runtime.CompilerServices;

namespace IConsoleLog;

public interface IConsoleLog
{
    public void WriteLog(string message = "", LogLevel logLevel = LogLevel.Info, [CallerMemberName] string caller = "");
    public void WriteCommandLog(string command, string message = "", LogLevel logLevel = LogLevel.Info);
}