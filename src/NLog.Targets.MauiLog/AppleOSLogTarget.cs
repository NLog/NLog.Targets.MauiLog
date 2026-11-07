#if __APPLE__

using System;
using NLog.Layouts;
using NLog.Targets.MauiLog;

namespace NLog.Targets
{
    /// <summary>
    /// Output target for Apple Unified Logging
    /// </summary>
    [Target("MauiLog")]
#if !NET
    [Foundation.Preserve (AllMembers = true)]
#endif
    public class AppleOSLogTarget : TargetWithLayoutHeaderAndFooter
    {
        private readonly IAppleOSLogger _appleOSLog;

        /// <summary>
        /// Not used at the moment
        /// </summary>
        public Layout? Category { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppleOSLogTarget"/> class.
        /// </summary>
        public AppleOSLogTarget()
            : this(new AppleOSLogger())
        {
        }

        private AppleOSLogTarget(IAppleOSLogger appleOSLog)
        {
            _appleOSLog = appleOSLog;
            Layout = "${logger}|${message:withException=true:exceptionSeparator=|}";
        }

        internal static AppleOSLogTarget Create(IAppleOSLogger appleOSLog) => new(appleOSLog);

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
            var logMessage = RenderLogEvent(layout, logEvent) ?? string.Empty;
            _appleOSLog.Log(logEvent.Level, logMessage);
        }

    }
}

#endif