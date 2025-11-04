# NLog.Targets.MauiLog
NLog Target for debugging on MAUI / Xamarin Mobile Platforms:

- Apple iOS / MacOS - Unified Logging OSLog (replacement of print and NSLog) 
- Android - Android.Util.Log / LogCat
- NetStandard - System.Diagnostics.Debugger.Log

[![Version](https://badge.fury.io/nu/NLog.Targets.MauiLog.svg)](https://www.nuget.org/packages/NLog.Targets.MauiLog)
[![AppVeyor](https://img.shields.io/appveyor/ci/nlog/nlog-targets-mauilog/master.svg)](https://ci.appveyor.com/project/nlog/nlog-targets-mauilog/branch/master)

## How to setup NLog in MAUI

1) **Install the NLog packages**

   - `Install-Package NLog.Targets.MauiLog` 
   - `Install-Package NLog.Extensions.Logging` 
    
   or in your csproj (Use ver. 8 for NET8, and ver. 7 for NET7 etc.):

    ```xml
    <PackageReference Include="NLog.Targets.MauiLog" Version="9.*" />
    <PackageReference Include="NLog.Extensions.Logging" Version="6.*" />
    ```

2) **Add NLog to the MauiApp**

   Update `MauiProgram.cs` to include NLog as Logging Provider: 
   ```csharp
   var builder = MauiApp.CreateBuilder();

   // Add NLog for Logging
   builder.Logging.ClearProviders();
   builder.Logging.AddNLog();
   ```

   If getting compiler errors with unknown methods, then update `using`-section:
   ```csharp
   using Microsoft.Extensions.Logging;
   using NLog;
   using NLog.Extensions.Logging;
   ```

3) **Load NLog configuration for logging**

   Add the `NLog.config`-file into the Application-project as assembly-resource (`Build Action` = `embedded resource`), and load like this:
   ```csharp
   NLog.LogManager.Setup().RegisterMauiLog()
       .LoadConfigurationFromAssemblyResource(typeof(App).Assembly);
   ```
   Alternative setup NLog configuration using [fluent-API](https://github.com/NLog/NLog/wiki/Fluent-Configuration-API):
   ```csharp
   var logger = NLog.LogManager.Setup().RegisterMauiLog()
                    .LoadConfiguration(c => c.ForLogger().FilterMinLevel(NLog.LogLevel.Debug).WriteToMauiLog())
                    .GetCurrentClassLogger();
   ```

## Configuration options for MAUI Log Target

- **Layout** - LogEvent message layout
- **Category** - LogEvent category layout (optional)

Example `NLog.config`-file:

```xml
<nlog>
<extensions>
    <add assembly="NLog.Targets.MauiLog" />
</extensions>
<targets>
    <target name="mauilog" type="MauiLog" />
</targets>
<rules>
    <logger name="*" minLevel="Info" writeTo="mauilog" />
</rules>
</nlog>
```

See also [Logging Unhandled Exceptions](https://github.com/NLog/NLog.Targets.MauiLog/wiki/Logging-Unhandled-Exceptions)
