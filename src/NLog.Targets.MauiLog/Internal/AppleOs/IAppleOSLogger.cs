namespace NLog.Targets.MauiLog;

internal interface IAppleOSLogger
{
    void Log(LogLevel level, string message);
}