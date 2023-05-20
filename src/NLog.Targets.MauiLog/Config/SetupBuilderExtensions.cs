using NLog.Config;

namespace NLog
{
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
    }
}
