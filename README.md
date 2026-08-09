# NLog.Targets.MauiLog
[![Version](https://badge.fury.io/nu/NLog.Targets.MauiLog.svg)](https://www.nuget.org/packages/NLog.Targets.MauiLog)
[![AppVeyor](https://img.shields.io/appveyor/ci/nlog/nlog-targets-mauilog/master.svg)](https://ci.appveyor.com/project/nlog/nlog-targets-mauilog/branch/master)

NLog Target for debugging on MAUI / Xamarin Mobile Platforms using the native logging systems:
- **Android** — `Android.Util.Log` → view in **Logcat**
- **iOS / macOS** — `OSLog` → view in **Xcode Console** or **Console.app**
- **Other platforms** — `System.Diagnostics.Debugger.Log` → view in the **debugger output**

## 1. Install NLog

Install the following NuGet packages:

```
dotnet package add NLog.Extensions.Logging
dotnet package add NLog.Targets.MauiLog
```

Use the package versions appropriate for your .NET version (Use ver. 10 for NET10, and ver. 8 for NET8 etc.).

## 2. Configure NLog

NLog integrates with the built-in Microsoft.Extensions.Logging infrastructure. Application code continues to use `ILogger<T>`, while NLog handles log routing, formatting, and writing to configured targets.

Configure NLog in `MauiProgram.cs`:

```csharp
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();

    builder.Logging.ClearProviders();

    // Register NLog MauiLog extension and setup as output target
    NLog.LogManager.Setup()
        .RegisterMauiLog()
        .LoadConfiguration(config => config
            .ForLogger()
            .FilterMinLevel(NLog.LogLevel.Info)
            .WriteToMauiLog());

    // Register NLog as Microsoft.Extensions.Logging provider
    builder.Logging.AddNLog();

    builder.UseMauiApp<App>();

    return builder.Build();
}
```

## 3. Start logging

NLog is now configured and receives log events from Microsoft.Extensions.Logging. Application code can continue using `ILogger<T>` as usual.

See also [Logging Unhandled Exceptions](https://github.com/NLog/NLog.Targets.MauiLog/wiki/Logging-Unhandled-Exceptions)

## Alternative configuration

The example above uses the [Fluent Configuration API](https://github.com/NLog/NLog/wiki/Fluent-Configuration-API). NLog also supports using configuration files like `NLog.config` or `appsettings.json`.

When switching from the Fluent Configuration API to loading a aconfiguration file, remove the `LoadConfiguration(...)` call from the setup.

When using configuration files, explicitly register the **MauiLog** extension:
```csharp
NLog.LogManager.Setup()
    .RegisterMauiLog();
```

This explicit registration is important when using trimming or Native AOT. If an NLog extension is only referenced through a configuration file, the linker can trim away the extension type, causing NLog logging to fail at runtime.

### NLog.config

Example `NLog.config` file:
```xml
<nlog throwConfigExceptions="true">
  <targets>
    <target name="mauilog" type="MauiLog" />
  </targets>
  <rules>
    <logger name="*" minLevel="Info" writeTo="mauilog" />
  </rules>
</nlog>
```

Make sure to add `NLog.config` file as an **Embedded resource** in the application project (`Build Action` = `Embedded resource`).

Load the embedded configuration during application startup:
```csharp
NLog.LogManager.Setup()
    .RegisterMauiLog()
    .LoadConfigurationFromAssemblyResource(typeof(App).Assembly);
```

### appsettings.json

NLog `AddNLog()` loads the `"NLog"` section when present, with environment overrides.

Example `appsettings.json` file:
```json
{
  "NLog": {
    "throwConfigExceptions": true,
    "targets": {
      "mauilog": {
        "type": "MauiLog"
      }
    },
    "rules": [
      {
        "logger": "*",
        "minLevel": "Info",
        "writeTo": "mauilog"
      }
    ]
  }
}
```
See also [NLog configuration with appsettings.json](https://github.com/NLog/NLog.Extensions.Logging/wiki/NLog-configuration-with-appsettings.json)
