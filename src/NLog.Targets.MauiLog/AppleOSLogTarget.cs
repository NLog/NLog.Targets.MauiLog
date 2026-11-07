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
        private readonly AppleOSLogTargetWriter _logWriter;

        /// <summary>
        /// Not used at the moment
        /// </summary>
        public Layout? Category { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppleOSLogTarget"/> class.
        /// </summary>
        public AppleOSLogTarget()
        {
            Layout = "${logger}|${message:withException=true:exceptionSeparator=|}";
            _logWriter = AppleOSLogTargetWriter.Create(new AppleOSLogAdapter());
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
            var logMessage = RenderLogEvent(layout, logEvent) ?? string.Empty;
            _logWriter.DebugWriteLine(logEvent.Level, logMessage);
        }

        private sealed class AppleOSLogAdapter : IAppleOSLog
        {
            public void Log(AppleOSLogLevel level, string message)
            {
                var nativeLevel = level == AppleOSLogLevel.Debug ? CoreFoundation.OSLogLevel.Debug
                    : level == AppleOSLogLevel.Info ? CoreFoundation.OSLogLevel.Info
                    : level == AppleOSLogLevel.Error ? CoreFoundation.OSLogLevel.Error
                    : CoreFoundation.OSLogLevel.Fault;

                CoreFoundation.OSLog.Default.Log(nativeLevel, message);
            }
        }
    }
}

#endif