# NLog.Targets.MauiLog
NLog Target for debugging on MAUI / Xamarin Mobile Platforms:

- Apple iOS / MacOS - Unified Logging
- Android - Android.Util.Log / LogCat
- NetStandard - System.Diagnostics.Debugger.Log

[![Version](https://badge.fury.io/nu/NLog.Targets.MauiLog.svg)](https://www.nuget.org/packages/NLog.Targets.MauiLog)
[![AppVeyor](https://img.shields.io/appveyor/ci/nlog/nlog-targets-mauilog/master.svg)](https://ci.appveyor.com/project/nlog/nlog-targets-mauilog/branch/master)

### How to use

1) Install the package

    `Install-Package NLog.Targets.MauiLog` or in your csproj:

    ```xml
    <PackageReference Include="NLog.Targets.MauiLog" Version="1.*" />
    ```

2) Add to your nlog.config:

    ```xml
    <extensions>
        <add assembly="NLog.Targets.MauiLog"/>
    </extensions>
    ```

3) Use the target "mauilog" in your nlog.config

    ```xml
    <targets>
        <target name="mauilog" xsi:type="MauiLog" />
    </targets>
    <rules>
        <logger minLevel="Info" writeTo="mauilog" />
    </rules>
    ```

### Configuration options for MAUI Log Target

- **Layout** - LogEvent message layout
- **Category** - LogEvent category layout (optional)
