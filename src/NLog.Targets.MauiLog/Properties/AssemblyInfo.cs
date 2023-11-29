using System.Security;

[assembly: NLog.Targets.MauiLog.Preserve]    // Automatic --linkskip=NLog.Targets.MauiLog
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityRules(SecurityRuleSet.Level1)]