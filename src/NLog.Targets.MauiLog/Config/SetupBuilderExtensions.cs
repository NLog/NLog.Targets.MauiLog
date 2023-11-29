using System;
using NLog.Config;
using NLog.Targets;

namespace NLog;

/// <summary>
/// Extension methods to setup LogFactory options
/// </summary>
public static class SetupBuilderExtensions
{
    /// <summary>
    /// Register the NLog.Web LayoutRenderers before loading NLog config
    /// </summary>
    public static ISetupBuilder RegisterMauiLog(this ISetupBuilder setupBuilder)
    {
        setupBuilder.SetupExtensions(e => e.RegisterMauiLog());
        return setupBuilder;
    }

    /// <summary>
    /// Register the NLog.Web LayoutRenderers before loading NLog config
    /// </summary>
    public static ISetupBuilder RegisterMauiLog(this ISetupBuilder setupBuilder, UnhandledExceptionEventHandler unhandledException)
    {
        if (unhandledException is null)
            throw new ArgumentNullException(nameof(unhandledException));

        setupBuilder.SetupExtensions(e => e.RegisterMauiLog());
        MauiExceptions.UnhandledException -= unhandledException;   // Avoid double registration
        MauiExceptions.UnhandledException += unhandledException;
        return setupBuilder;
    }
}