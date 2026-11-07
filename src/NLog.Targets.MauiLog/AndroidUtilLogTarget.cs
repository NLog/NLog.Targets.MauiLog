#if __ANDROID__

using System;
using NLog.Layouts;
using NLog.Targets.MauiLog;

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
        private readonly AndroidLogTargetWriter _logWriter;

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
            _logWriter = AndroidLogTargetWriter.Create(new AndroidLogAdapter());
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
            _logWriter.DebugWriteLine(logEvent.Level, category, message, logEvent.Exception);
        }

        private sealed class AndroidLogAdapter : IAndroidLog
        {
            public void Log(AndroidLogLevel level, string category, string message, Exception? exception)
            {
                var throwable = exception is null ? null : Java.Lang.Throwable.FromException(exception);

                if (level == AndroidLogLevel.Verbose)
                {
                    if (throwable is null)
                        Android.Util.Log.Verbose(category, message);
                    else
                        Android.Util.Log.Verbose(category, throwable, message);
                }
                else if (level == AndroidLogLevel.Debug)
                {
                    if (throwable is null)
                        Android.Util.Log.Debug(category, message);
                    else
                        Android.Util.Log.Debug(category, throwable, message);
                }
                else if (level == AndroidLogLevel.Info)
                {
                    if (throwable is null)
                        Android.Util.Log.Info(category, message);
                    else
                        Android.Util.Log.Info(category, throwable, message);
                }
                else if (level == AndroidLogLevel.Warn)
                {
                    if (throwable is null)
                        Android.Util.Log.Warn(category, message);
                    else
                        Android.Util.Log.Warn(category, throwable, message);
                }
                else if (level == AndroidLogLevel.Error)
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
}

#endif