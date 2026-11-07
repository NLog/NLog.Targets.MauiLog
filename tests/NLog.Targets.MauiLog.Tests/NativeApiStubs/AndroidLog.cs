namespace Android.Util;

public static class Log
{
    public static string? LastMethod { get; private set; }
    public static string? LastCategory { get; private set; }
    public static string? LastMessage { get; private set; }
    public static Java.Lang.Throwable? LastThrowable { get; private set; }

    public static void Reset()
    {
        LastMethod = null;
        LastCategory = null;
        LastMessage = null;
        LastThrowable = null;
    }

    public static int Verbose(string category, string message) => Record("Verbose", category, null, message);
    public static int Verbose(string category, Java.Lang.Throwable throwable, string message) => Record("Verbose", category, throwable, message);
    public static int Debug(string category, string message) => Record("Debug", category, null, message);
    public static int Debug(string category, Java.Lang.Throwable throwable, string message) => Record("Debug", category, throwable, message);
    public static int Info(string category, string message) => Record("Info", category, null, message);
    public static int Info(string category, Java.Lang.Throwable throwable, string message) => Record("Info", category, throwable, message);
    public static int Warn(string category, string message) => Record("Warn", category, null, message);
    public static int Warn(string category, Java.Lang.Throwable throwable, string message) => Record("Warn", category, throwable, message);
    public static int Error(string category, string message) => Record("Error", category, null, message);
    public static int Error(string category, Java.Lang.Throwable throwable, string message) => Record("Error", category, throwable, message);
    public static int Wtf(string category, string message) => Record("Wtf", category, null, message);
    public static int Wtf(string category, Java.Lang.Throwable throwable, string message) => Record("Wtf", category, throwable, message);

    private static int Record(string method, string category, Java.Lang.Throwable? throwable, string message)
    {
        LastMethod = method;
        LastCategory = category;
        LastThrowable = throwable;
        LastMessage = message;
        return 0;
    }
}