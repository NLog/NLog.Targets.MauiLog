using NLog;
using NLog.Targets.MauiLog;
using Xunit;

namespace NLog.Targets.MauiLog.Tests;

public class NativeTargetWriterTests
{
    public static TheoryData<LogLevel, string> AndroidLogLevels => new()
    {
        { LogLevel.Trace, "Verbose" },
        { LogLevel.Debug, "Debug" },
        { LogLevel.Info, "Info" },
        { LogLevel.Warn, "Warn" },
        { LogLevel.Error, "Error" },
        { LogLevel.Fatal, "Wtf" }
    };

    public static TheoryData<LogLevel, string> AppleLogLevels => new()
    {
        { LogLevel.Trace, "Debug" },
        { LogLevel.Debug, "Debug" },
        { LogLevel.Info, "Info" },
        { LogLevel.Warn, "Error" },
        { LogLevel.Error, "Error" },
        { LogLevel.Fatal, "Fault" }
    };

    [Theory]
    [MemberData(nameof(AndroidLogLevels))]
    public void AndroidWriter_MapsLevelsAndPreservesMessage(LogLevel level, string expectedLevel)
    {
        var log = new RecordingAndroidLog();
        var writer = AndroidLogTargetWriter.Create(log);

        writer.DebugWriteLine(level, "category", "message", null);

        Assert.Equal(expectedLevel, log.Level.ToString());
        Assert.Equal("category", log.Category);
        Assert.Equal("message", log.Message);
        Assert.Null(log.Exception);
    }

    [Fact]
    public void AndroidWriter_UnwrapsSingleExceptionAndFlattensMultipleExceptions()
    {
        var log = new RecordingAndroidLog();
        var writer = AndroidLogTargetWriter.Create(log);
        var inner = new InvalidOperationException("single");
        writer.DebugWriteLine(LogLevel.Error, "category", "message", new AggregateException(inner));
        Assert.Same(inner, log.Exception);

        writer.DebugWriteLine(LogLevel.Error, "category", "message", new AggregateException(new Exception("one"), new Exception("two")));
        var flattened = Assert.IsType<AggregateException>(log.Exception);
        Assert.Equal(2, flattened.InnerExceptions.Count);
    }

    [Theory]
    [MemberData(nameof(AppleLogLevels))]
    public void AppleWriter_MapsLevelsAndPreservesMessage(LogLevel level, string expectedLevel)
    {
        var log = new RecordingAppleLog();
        var writer = AppleOSLogTargetWriter.Create(log);

        writer.DebugWriteLine(level, "message");

        Assert.Equal(expectedLevel, log.Level.ToString());
        Assert.Equal("message", log.Message);
    }

    private sealed class RecordingAndroidLog : IAndroidLog
    {
        public AndroidLogLevel Level { get; private set; }
        public string? Category { get; private set; }
        public string? Message { get; private set; }
        public Exception? Exception { get; private set; }

        public void Log(AndroidLogLevel level, string category, string message, Exception? exception)
        {
            Level = level;
            Category = category;
            Message = message;
            Exception = exception;
        }
    }

    private sealed class RecordingAppleLog : IAppleOSLog
    {
        public AppleOSLogLevel Level { get; private set; }
        public string? Message { get; private set; }

        public void Log(AppleOSLogLevel level, string message)
        {
            Level = level;
            Message = message;
        }
    }
}