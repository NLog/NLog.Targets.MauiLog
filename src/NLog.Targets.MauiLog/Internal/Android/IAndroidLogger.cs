using System;

namespace NLog.Targets.MauiLog;

internal interface IAndroidLogger
{
    void Log(LogLevel level, string category, string message, Exception? exception);
}