using System;
using System.Diagnostics;
using Layout = NLog.Layouts.Layout;

namespace NLog.Targets;

/// <summary>
/// Output target for System.Diagnostics.Debugger.Log with fallback to Console when no debugger
/// </summary>
[Target("MauiLog")]
[NLog.Targets.MauiLogLinker.Preserve]
public class MauiLogTarget : TargetWithLayoutHeaderAndFooter
{
	/// <summary>
	/// The category of the message
	/// </summary>
	public Layout Category { get; set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="MauiLogTarget"/> class.
	/// </summary>
	public MauiLogTarget()
	{
		Layout = "${level:padding=-5:uppercase=true}: ${message:withException=true:exceptionSeparator=|}";
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
		var logMessage = RenderLogEvent(layout, logEvent) ?? string.Empty;
		if (Debugger.IsLogging())
		{
			var logCategory = RenderLogEvent(Category, logEvent);
			if (string.IsNullOrEmpty(logCategory))
				logCategory = null;
			Debugger.Log(logEvent.Level.Ordinal, logCategory, logMessage + Environment.NewLine);
		}
		else
		{
			Console.WriteLine(logMessage);
		}
	}
}