using System.Security;

[assembly: NLog.Targets.MauiLogLinker.Preserve]    // Automatic --linkskip=NLog.Targets.MauiLog
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityRules(SecurityRuleSet.Level1)]