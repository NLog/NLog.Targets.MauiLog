using NLog.Config;
using NLog.Targets;

namespace NLog
{
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
#if __ANDROID__
            return setupBuilder.RegisterTarget<AndroidUtilLogTarget>("MauiLog");
#elif __APPLE__
            return setupBuilder.RegisterTarget<AppleOSLogTarget>("MauiLog");
#else
            return setupBuilder.RegisterTarget<ConsoleDebuggerTarget>("MauiLog");
#endif
        }
    }
}
