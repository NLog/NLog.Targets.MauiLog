using System.Security;
using NLog.Targets.MauiLog;

[assembly: Preserve]    // Automatic --linkskip=NLog.Targets.MauiLog
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityRules(SecurityRuleSet.Level1)]