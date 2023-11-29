using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace NLog;

/// <summary>
/// Extension methods to setup NLog <see cref="LoggingConfiguration"/>
/// </summary>
public static class SetupLoadConfigurationExtensions
{
    /// <summary>
    /// Write to MAUI NLog Target
    /// </summary>
    /// <param name="configBuilder"></param>
    /// <param name="layout">Override the default Layout for output</param>
    /// <param name="category">Override the logging category</param>
    public static ISetupConfigurationTargetBuilder WriteToMauiLog(this ISetupConfigurationTargetBuilder configBuilder, NLog.Layouts.Layout layout = null, NLog.Layouts.Layout category = null)
    {
#if __ANDROID__
        var logTarget = new MauiLogTarget();
#else
	    var logTarget = new MauiLogTarget();
#endif
		// var logTarget = new MauiLogTarget();

		if (layout != null)
            logTarget.Layout = layout;
        if (category != null)
            logTarget.Category = category;
        return configBuilder.WriteTo(logTarget);
    }

    /// <summary>
    /// Write to MAUI NLog Target (when DEBUG-build)
    /// </summary>
    /// <param name="configBuilder"></param>
    /// <param name="layout">Override the default Layout for output</param>
    /// <param name="category">Override the logging category</param>
    [System.Diagnostics.Conditional("DEBUG")]
    public static void WriteToMauiLogConditional(this ISetupConfigurationTargetBuilder configBuilder, NLog.Layouts.Layout layout = null, NLog.Layouts.Layout category = null)
    {
        configBuilder.WriteToMauiLog(layout, category);
    }
}