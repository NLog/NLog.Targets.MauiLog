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
        private readonly IAndroidLogger _androidLog;

        /// <summary>
        /// Source of a log message
        /// </summary>
        public Layout Category { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidUtilLogTarget"/> class.
        /// </summary>
        public AndroidUtilLogTarget()
            : this(new AndroidLogger())
        {
        }

        private AndroidUtilLogTarget(IAndroidLogger androidLog)
        {
            _androidLog = androidLog;
            Layout = "${message}";
            Category = "${logger}";
        }

        internal static AndroidUtilLogTarget Create(IAndroidLogger androidLog) => new(androidLog);

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
            _androidLog.Log(logEvent.Level, category, message, logEvent.Exception);
        }

    }
}

#endif