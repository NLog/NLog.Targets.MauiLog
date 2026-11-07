using System;

namespace NLog.Targets.MauiLog;

internal enum AndroidLogLevel
{
    Verbose,
    Debug,
    Info,
    Warn,
    Error,
    Wtf
}

internal interface IAndroidLog
{
    void Log(AndroidLogLevel level, string category, string message, Exception? exception);
}

internal sealed class AndroidLogTargetWriter
{
    private readonly IAndroidLog _androidLog;

    private AndroidLogTargetWriter(IAndroidLog androidLog)
    {
        _androidLog = androidLog ?? throw new ArgumentNullException(nameof(androidLog));
    }

    internal static AndroidLogTargetWriter Create(IAndroidLog androidLog) => new(androidLog);

    internal void DebugWriteLine(LogLevel level, string category, string message, Exception? exception)
    {
        if (exception is AggregateException aggregateException)
        {
            if (aggregateException.InnerExceptions.Count == 1 && aggregateException.InnerExceptions[0] is not AggregateException)
                exception = aggregateException.InnerExceptions[0];
            else
                exception = aggregateException.Flatten();
        }

        var androidLevel = level == LogLevel.Trace ? AndroidLogLevel.Verbose
            : level == LogLevel.Debug ? AndroidLogLevel.Debug
            : level == LogLevel.Info ? AndroidLogLevel.Info
            : level == LogLevel.Warn ? AndroidLogLevel.Warn
            : level == LogLevel.Error ? AndroidLogLevel.Error
            : AndroidLogLevel.Wtf;

        _androidLog.Log(androidLevel, category, message, exception);
    }
}