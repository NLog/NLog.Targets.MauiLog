#if __ANDROID__

using System;
using NLog.Layouts;

namespace NLog.Targets
{
    /// <summary>
    /// Output target for Android.Util.Log
    /// </summary>
    [Target("MauiLog")]
#if !NET
    [Android.Runtime.Preserve (AllMembers = true)]
#endif
    public class AndroidUtilLogTarget : TargetWithLayoutHeaderAndFooter
    {
        /// <summary>
        /// Source of a log message
        /// </summary>
        public Layout Category { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidUtilLogTarget"/> class.
        /// </summary>
        public AndroidUtilLogTarget()
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

        private void DebugWriteLine(Layout layout, LogEventInfo logEvent)
        {
            var message = RenderLogEvent(layout, logEvent) ?? string.Empty;
            var category = RenderLogEvent(Category, logEvent) ?? string.Empty;

            Java.Lang.Throwable? throwable = null;
            if (logEvent.Exception != null)
            {
                throwable = Java.Lang.Throwable.FromException(logEvent.Exception);
            }

            if (logEvent.Level == LogLevel.Trace)
            {
                if (throwable is null)
                    Android.Util.Log.Verbose(category, message);
                else
                    Android.Util.Log.Verbose(category, throwable, message);
            }
            else if (logEvent.Level == LogLevel.Debug)
            {
                if (throwable is null)
                    Android.Util.Log.Debug(category, message);
                else
                    Android.Util.Log.Debug(category, throwable, message);
            }
            else if (logEvent.Level == LogLevel.Info)
            {
                if (throwable is null)
                    Android.Util.Log.Info(category, message);
                else
                    Android.Util.Log.Info(category, throwable, message);
            }
            else if (logEvent.Level == LogLevel.Warn)
            {
                if (throwable is null)
                    Android.Util.Log.Warn(category, message);
                else
                    Android.Util.Log.Warn(category, throwable, message);
            }
            else if (logEvent.Level == LogLevel.Error)
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
}

#endif