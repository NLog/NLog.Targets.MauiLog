#if __APPLE__

namespace NLog.Targets.MauiLog;

internal sealed class AppleOSLogger : IAppleOSLogger
{
    public void Log(LogLevel level, string message)
    {
        var nativeLevel = level == LogLevel.Trace || level == LogLevel.Debug ? CoreFoundation.OSLogLevel.Debug
            : level == LogLevel.Info ? CoreFoundation.OSLogLevel.Info
            : level == LogLevel.Warn || level == LogLevel.Error ? CoreFoundation.OSLogLevel.Error
            : CoreFoundation.OSLogLevel.Fault;

        CoreFoundation.OSLog.Default.Log(nativeLevel, message);
    }
}

#endif