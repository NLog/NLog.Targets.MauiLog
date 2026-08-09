namespace NLog.Targets.MauiLog
{
    using System;

    internal static class MauiExceptions
    {
        // We'll route all unhandled exceptions through this one event.
        public static event UnhandledExceptionEventHandler? UnhandledException;

        static MauiExceptions()
        {
            // General .NET unhandled-exception notification.
            // Platform runtimes may have additional exception paths.
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                UnhandledException?.Invoke(sender, args);
            };

#if __ANDROID__
            // Android has a separate unhandled-exception notification.
            if (!OperatingSystem.IsAndroidVersionAtLeast(21))
                return;

            Android.Runtime.AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) =>
            {
#pragma warning disable CS8604 // Possible null reference argument for parameter 'sender'
                UnhandledException?.Invoke(sender, new UnhandledExceptionEventArgs(args.Exception, true));
#pragma warning restore CS8604 // Possible null reference argument for parameter 'sender'
            };
#endif
        }
    }
}
