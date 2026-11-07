using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using Xunit;

namespace NLog.Targets.MauiLog.Tests;

public class MauiLogConfigurationTests
{
    [Fact]
    public void RegisterMauiLog_AcceptsBothSetupOverloadsAndRejectsNullHandler()
    {
        // Arrange
        using var logFactory = new LogFactory();
        UnhandledExceptionEventHandler handler = (sender, args) => { };

        try
        {
            // Act
            logFactory.Setup().RegisterMauiLog();
            logFactory.Setup().RegisterMauiLog(handler);
            logFactory.Setup().RegisterMauiLog(handler);

            // Assert
            Assert.Throws<ArgumentNullException>(() => logFactory.Setup().RegisterMauiLog(null!));
        }
        finally
        {
            MauiExceptions.UnhandledException -= handler;
        }
    }

    [Fact]
    public void WriteToMauiLog_UsesConsoleDebuggerTargetAndConfiguredLayouts()
    {
        // Arrange
        using var logFactory = new LogFactory();
        var layout = Layout.FromString("${message}");
        var category = Layout.FromString("${logger}");

        // Act
        logFactory.Setup().LoadConfiguration(configuration =>
            configuration.ForLogger().WriteToMauiLog(layout, category));

        // Assert
        var configuration = Assert.IsType<LoggingConfiguration>(logFactory.Configuration);
        var target = Assert.IsType<ConsoleDebuggerTarget>(Assert.Single(configuration.AllTargets));

        Assert.Same(layout, target.Layout);
        Assert.Same(category, target.Category);
    }

    [Fact]
    public void WriteToMauiLog_UsesDefaultLayoutsWhenOverridesAreOmitted()
    {
        // Arrange
        using var logFactory = new LogFactory();

        // Act
        logFactory.Setup().LoadConfiguration(configuration =>
            configuration.ForLogger().WriteToMauiLog());

        // Assert
        var loggingConfiguration = Assert.IsType<LoggingConfiguration>(logFactory.Configuration);
        var target = Assert.IsType<ConsoleDebuggerTarget>(Assert.Single(loggingConfiguration.AllTargets));
        Assert.Equal("${level:padding=-5:uppercase=true}: ${message:withException=true:exceptionSeparator=|}", target.Layout.ToString());
        Assert.Equal("${logger}", target.Category.ToString());
    }

    [Fact]
    public void ConsoleDebuggerTarget_WritesHeaderMessageAndFooter()
    {
        // Arrange
        using var logFactory = new LogFactory();
        var target = new ConsoleDebuggerTarget
        {
            Header = Layout.FromString("header"),
            Footer = Layout.FromString("footer")
        };
        var loggingConfiguration = new LoggingConfiguration();
        loggingConfiguration.AddTarget("maui", target);
        loggingConfiguration.AddRuleForAllLevels(target);

        using var output = new StringWriter();
        var originalOutput = Console.Out;
        try
        {
            // Act
            Console.SetOut(output);
            logFactory.Configuration = loggingConfiguration;
            logFactory.GetLogger("unit-test").Info("message");
            logFactory.Shutdown();
        }
        finally
        {
            Console.SetOut(originalOutput);
        }

        // Assert
        Assert.Contains("header", output.ToString());
        Assert.Contains("message", output.ToString());
        Assert.Contains("footer", output.ToString());
    }
}