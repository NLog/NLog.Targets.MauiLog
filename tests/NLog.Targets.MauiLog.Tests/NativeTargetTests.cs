using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using NLog.Targets.MauiLog;
using Xunit;

namespace NLog.Targets.MauiLog.Tests;

public class NativeTargetTests
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
    public void AndroidAdapter_MapsLevelsAndPreservesMessage(LogLevel level, string expectedLevel)
    {
        // Arrange
        Android.Util.Log.Reset();

        // Act
        new AndroidLogger().Log(level, "category", "message", null);

        // Assert
        Assert.Equal(expectedLevel, Android.Util.Log.LastMethod);
        Assert.Equal("category", Android.Util.Log.LastCategory);
        Assert.Equal("message", Android.Util.Log.LastMessage);
        Assert.Null(Android.Util.Log.LastThrowable);
    }

    [Fact]
    public void AndroidAdapter_UnwrapsSingleExceptionAndFlattensMultipleExceptions()
    {
        // Arrange
        var adapter = new AndroidLogger();
        var inner = new InvalidOperationException("single");

        // Act
        adapter.Log(LogLevel.Error, "category", "message", new AggregateException(inner));

        // Assert
        Assert.Same(inner, Android.Util.Log.LastThrowable!.SourceException);

        // Act
        adapter.Log(LogLevel.Error, "category", "message", new AggregateException(new Exception("one"), new Exception("two")));

        // Assert
        var flattened = Assert.IsType<AggregateException>(Android.Util.Log.LastThrowable!.SourceException);
        Assert.Equal(2, flattened.InnerExceptions.Count);
    }

    [Fact]
    public void AndroidTarget_UsesInjectedLogForOpenWriteAndClose()
    {
        // Arrange
        using var logFactory = new LogFactory();
        var log = new RecordingNativeLog();
        var target = AndroidUtilLogTarget.Create(log);
        target.Header = Layout.FromString("header");
        target.Footer = Layout.FromString("footer");

        var configuration = new LoggingConfiguration();
        configuration.AddTarget("android", target);
        configuration.AddRuleForAllLevels(target);

        // Act
        logFactory.Configuration = configuration;
        logFactory.GetLogger("unit-test").Info("body");
        logFactory.Shutdown();

        // Assert
        Assert.Collection(log.AndroidCalls,
            call => Assert.Equal(("Info", "Starting", "header"), (call.Level.ToString(), call.Category, call.Message)),
            call => Assert.Equal(("Info", "unit-test", "body"), (call.Level.ToString(), call.Category, call.Message)),
            call => Assert.Equal(("Info", "Closing", "footer"), (call.Level.ToString(), call.Category, call.Message)));
    }

    [Theory]
    [MemberData(nameof(AppleLogLevels))]
    public void AppleAdapter_MapsLevelsAndPreservesMessage(LogLevel level, string expectedLevel)
    {
        // Arrange
        CoreFoundation.OSLog.Default.Reset();

        // Act
        new AppleOSLogger().Log(level, "message");

        // Assert
        Assert.Equal(expectedLevel, CoreFoundation.OSLog.Default.LastLevel.ToString());
        Assert.Equal("message", CoreFoundation.OSLog.Default.LastMessage);
    }

    [Fact]
    public void AppleTarget_UsesInjectedLogForOpenWriteAndClose()
    {
        // Arrange
        using var logFactory = new LogFactory();
        var log = new RecordingNativeLog();
        var target = AppleOSLogTarget.Create(log);
        target.Layout = Layout.FromString("${message}");
        target.Header = Layout.FromString("header");
        target.Footer = Layout.FromString("footer");

        var configuration = new LoggingConfiguration();
        configuration.AddTarget("apple", target);
        configuration.AddRuleForAllLevels(target);

        // Act
        logFactory.Configuration = configuration;
        logFactory.GetLogger("unit-test").Info("body");
        logFactory.Shutdown();

        // Assert
        Assert.Collection(log.AppleCalls,
            call => Assert.Equal(("Info", "header"), (call.Level.ToString(), call.Message)),
            call => Assert.Equal(("Info", "body"), (call.Level.ToString(), call.Message)),
            call => Assert.Equal(("Info", "footer"), (call.Level.ToString(), call.Message)));
    }

    private sealed class RecordingNativeLog : IAndroidLogger, IAppleOSLogger
    {
        public List<(LogLevel Level, string Category, string Message)> AndroidCalls { get; } = new();
        public List<(LogLevel Level, string Message)> AppleCalls { get; } = new();

        public void Log(LogLevel level, string category, string message, Exception? exception)
        {
            AndroidCalls.Add((level, category, message));
        }

        public void Log(LogLevel level, string message)
        {
            AppleCalls.Add((level, message));
        }
    }
}