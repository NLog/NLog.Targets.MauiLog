using NLog;
using NLog.Targets.MauiLog;
using Xunit;

namespace NLog.Targets.MauiLog.Tests;

public class MauiExceptionsTests
{
    [Fact]
    public void RegisterMauiLog_ForwardsUnhandledExceptionToSubscriber()
    {
        // Arrange
        using var logFactory = new LogFactory();
        var expectedSender = new object();
        var expectedException = new InvalidOperationException("unhandled");
        var expectedArgs = new UnhandledExceptionEventArgs(expectedException, false);
        object? actualSender = null;
        UnhandledExceptionEventArgs? actualArgs = null;
        UnhandledExceptionEventHandler handler = (sender, args) =>
        {
            actualSender = sender;
            actualArgs = args;
        };

        logFactory.Setup().RegisterMauiLog(handler);
        try
        {
            // Act
            MauiExceptions.RaiseUnhandledException(expectedSender, expectedArgs);
        }
        finally
        {
            MauiExceptions.UnhandledException -= handler;
        }

        // Assert
        Assert.Same(expectedSender, actualSender);
        Assert.Same(expectedArgs, actualArgs);
        Assert.Same(expectedException, actualArgs!.ExceptionObject);
        Assert.False(actualArgs.IsTerminating);
    }
}