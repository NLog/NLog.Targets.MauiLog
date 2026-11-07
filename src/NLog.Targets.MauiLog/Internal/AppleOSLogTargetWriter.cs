using System;

namespace NLog.Targets.MauiLog;

internal enum AppleOSLogLevel
{
    Debug,
    Info,
    Error,
    Fault
}

internal interface IAppleOSLog
{
    void Log(AppleOSLogLevel level, string message);
}

internal sealed class AppleOSLogTargetWriter
{
    private readonly IAppleOSLog _appleOSLog;

    private AppleOSLogTargetWriter(IAppleOSLog appleOSLog)
    {
        _appleOSLog = appleOSLog ?? throw new ArgumentNullException(nameof(appleOSLog));
    }

    internal static AppleOSLogTargetWriter Create(IAppleOSLog appleOSLog) => new(appleOSLog);

    internal void DebugWriteLine(LogLevel level, string message)
    {
        var appleLevel = level == LogLevel.Trace || level == LogLevel.Debug ? AppleOSLogLevel.Debug
            : level == LogLevel.Info ? AppleOSLogLevel.Info
            : level == LogLevel.Warn || level == LogLevel.Error ? AppleOSLogLevel.Error
            : AppleOSLogLevel.Fault;

        _appleOSLog.Log(appleLevel, message);
    }
}