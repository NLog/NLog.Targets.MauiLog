using Android.OS;

namespace NLog.Targets;
    
/// <summary>
/// Output target for Android.Util.Log
/// </summary>
[Target("MauiLog")]
[NLog.Targets.MauiLogLinker.Preserve]
public class MauiLogTarget : TargetWithLayoutHeaderAndFooter
{
    /// <summary>
    /// Source of a log message
    /// </summary>
    public NLog.Layouts.Layout Category { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MauiLogTarget"/> class.
    /// </summary>
    public MauiLogTarget()
    {
        Layout = "${message}";
        Category = "${logger}";
    }

    /// <inheritdoc/>
    protected override void InitializeTarget()
    {
        base.InitializeTarget();

        if (Header != null)
        {
            var logEvent = LogEventInfo.CreateNullEvent();
            logEvent.Level = LogLevel.Info;
            logEvent.LoggerName = "Starting";
            DebugWriteLine(Header, logEvent);
        }
    }

    /// <inheritdoc/>
    protected override void CloseTarget()
    {
        if (Footer != null)
        {
            var logEvent = LogEventInfo.CreateNullEvent();
            logEvent.Level = LogLevel.Info;
            logEvent.LoggerName = "Closing";
            DebugWriteLine(Footer, logEvent);
        }

        base.CloseTarget();
    }

    /// <inheritdoc/>
    protected override void Write(LogEventInfo logEvent)
    {
        DebugWriteLine(Layout, logEvent);
    }

    private void DebugWriteLine(NLog.Layouts.Layout layout, LogEventInfo logEvent)
    {
        var message = RenderLogEvent(layout, logEvent) ?? string.Empty;
        var category = RenderLogEvent(Category, logEvent);
        if (string.IsNullOrEmpty(category))
            category = null;

        if (!OperatingSystem.IsAndroidVersionAtLeast(21, 0, 0, 0))
        {
            System.Diagnostics.Debug.WriteLine(message);
            return;
        }

        Java.Lang.Throwable throwable = null;
        if (logEvent.Exception != null)
        {
            throwable = Java.Lang.Throwable.FromException(logEvent.Exception);
        }

        if (logEvent.Level == LogLevel.Trace)
        {
            Android.Util.Log.Verbose(category, throwable, message);
        }
        else if (logEvent.Level == LogLevel.Debug)
        {
            Android.Util.Log.Debug(category, throwable, message);
        }
        else if (logEvent.Level == LogLevel.Info)
        {
            Android.Util.Log.Info(category, throwable, message);
        }
        else if (logEvent.Level == LogLevel.Warn)
        {
            Android.Util.Log.Warn(category, throwable, message);
        }
        else if (logEvent.Level == LogLevel.Error)
        {
            Android.Util.Log.Error(category, throwable, message);
        }
        else
        {
            Android.Util.Log.Wtf(category, throwable, message);
        }
    }
}