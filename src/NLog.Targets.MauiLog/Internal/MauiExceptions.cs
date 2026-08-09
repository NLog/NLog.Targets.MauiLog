namespace NLog.Targets.MauiLog
{
    using System;

    internal static class MauiExceptions
    {
        // Provides a unified event for the platform-specific unhandled
        // managed exception notifications supported by this library.
        public static event UnhandledExceptionEventHandler? UnhandledException;

        static MauiExceptions()
        {
            // General .NET unhandled-exception notification.
            // Some platforms have additional exception handling mechanisms.
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                UnhandledException?.Invoke(sender, args);
            };

#if __ANDROID__
            // Android provides a separate notification for unhandled managed
            // exceptions that are raised through the Android runtime.
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
