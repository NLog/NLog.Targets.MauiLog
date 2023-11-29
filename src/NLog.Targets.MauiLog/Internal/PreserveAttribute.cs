namespace NLog.Targets.MauiLog;

using System;

/// <summary>
/// Prevents the Xamarin linker from linking the target.
/// </summary>
/// <remarks>
/// By applying this attribute all of the members of the target will be kept as if they had been referenced by the code.
/// </remarks>
[AttributeUsage(AttributeTargets.All)]
public sealed class PreserveAttribute : Attribute
{
	/// <summary>
	/// Ensures that all members of this type are preserved
	/// </summary>
	public bool AllMembers { get; set; } = true;
	/// <summary>
	/// Flags the method as a method to preserve during linking if the container class is pulled in.
	/// </summary>
	public bool Conditional { get; set; }
}