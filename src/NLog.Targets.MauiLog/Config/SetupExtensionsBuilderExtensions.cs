using NLog.Config;
using NLog.Targets;

namespace NLog;

/// <summary>
/// Extension methods to setup NLog extensions, so they are known when loading NLog LoggingConfiguration
/// </summary>
public static class SetupExtensionsBuilderExtensions
{
    /// <summary>
    /// Register the NLog.Web LayoutRenderers
    /// </summary>
    public static ISetupExtensionsBuilder RegisterMauiLog(this ISetupExtensionsBuilder setupBuilder)
    {
        return setupBuilder.RegisterTarget<MauiLogTarget>("MauiLog");
    }
}