using System.Security;using NLog.Internal.Xamarin;

[assembly: Preserve]    // Automatic --linkskip=NLog.Targets.MauiLog
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityRules(SecurityRuleSet.Level1)]