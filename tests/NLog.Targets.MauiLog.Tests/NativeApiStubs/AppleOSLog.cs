namespace CoreFoundation;

public sealed class OSLog
{
    public static OSLog Default { get; } = new();
    public OSLogLevel? LastLevel { get; private set; }
    public string? LastMessage { get; private set; }

    public void Reset()
    {
        LastLevel = null;
        LastMessage = null;
    }

    public void Log(OSLogLevel level, string message)
    {
        LastLevel = level;
        LastMessage = message;
    }
}