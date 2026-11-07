#if __ANDROID__

using System;

namespace NLog.Targets.MauiLog;

internal sealed class AndroidLogger : IAndroidLogger
{
    public void Log(LogLevel level, string category, string message, Exception? exception)
    {
        if (exception is AggregateException aggregateException)
        {
            if (aggregateException.InnerExceptions.Count == 1 && aggregateException.InnerExceptions[0] is not AggregateException)
                exception = aggregateException.InnerExceptions[0];
            else
                exception = aggregateException.Flatten();
        }

        var throwable = exception is null ? null : Java.Lang.Throwable.FromException(exception);

        if (level == LogLevel.Trace)
        {
            if (throwable is null)
                Android.Util.Log.Verbose(category, message);
            else
                Android.Util.Log.Verbose(category, throwable, message);
        }
        else if (level == LogLevel.Debug)
        {
            if (throwable is null)
                Android.Util.Log.Debug(category, message);
            else
                Android.Util.Log.Debug(category, throwable, message);
        }
        else if (level == LogLevel.Info)
        {
            if (throwable is null)
                Android.Util.Log.Info(category, message);
            else
                Android.Util.Log.Info(category, throwable, message);
        }
        else if (level == LogLevel.Warn)
        {
            if (throwable is null)
                Android.Util.Log.Warn(category, message);
            else
                Android.Util.Log.Warn(category, throwable, message);
        }
        else if (level == LogLevel.Error)
        {
            if (throwable is null)
                Android.Util.Log.Error(category, message);
            else
                Android.Util.Log.Error(category, throwable, message);
        }
        else
        {
            if (throwable is null)
                Android.Util.Log.Wtf(category, message);
            else
                Android.Util.Log.Wtf(category, throwable, message);
        }
    }
}

#endif