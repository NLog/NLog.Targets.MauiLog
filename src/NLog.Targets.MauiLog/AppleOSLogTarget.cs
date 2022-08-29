#if __APPLE__

using System;
using NLog.Layouts;

namespace NLog.Targets
{
    /// <summary>
    /// Output target for Apple Unified Logging
    /// </summary>
    [Target("MauiLog")]
    public class AppleOSLogTarget : TargetWithLayoutHeaderAndFooter
    {
        /// <summary>
        /// Not used at the moment
        /// </summary>
        public Layout Category { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppleOSLogTarget"/> class.
        /// </summary>
        public AppleOSLogTarget()
        {
            Layout = "${logger}|${message:withException=true:exceptionSeparator=|}";
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

        private void DebugWriteLine(Layout layout, LogEventInfo logEvent)
        {
            var logMessage = RenderLogEvent(layout, logEvent) ?? string.Empty;
            if (logEvent.Level == LogLevel.Trace || logEvent.Level == LogLevel.Debug)
            {
                CoreFoundation.OSLog.Default.Log(CoreFoundation.OSLogLevel.Debug, logMessage);
            }
            else if (logEvent.Level == LogLevel.Info)
            {
                CoreFoundation.OSLog.Default.Log(CoreFoundation.OSLogLevel.Info, logMessage);
            }
            else if (logEvent.Level == LogLevel.Warn || logEvent.Level == LogLevel.Error)
            {
                CoreFoundation.OSLog.Default.Log(CoreFoundation.OSLogLevel.Error, logMessage);
            }
            else
            {
                CoreFoundation.OSLog.Default.Log(CoreFoundation.OSLogLevel.Fault, logMessage);
            }
        }
    }
}

#endif