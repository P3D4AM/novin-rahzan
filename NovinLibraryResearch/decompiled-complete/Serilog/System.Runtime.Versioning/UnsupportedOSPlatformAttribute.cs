using System.Diagnostics;

namespace System.Runtime.Versioning;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
[Conditional("MULTI_TARGETING_SUPPORT_ATTRIBUTES")]
internal sealed class UnsupportedOSPlatformAttribute : Attribute
{
	public string? Message { get; }

	public UnsupportedOSPlatformAttribute(string platformName)
	{
	}

	public UnsupportedOSPlatformAttribute(string platformName, string? message)
	{
		Message = message;
	}
}
