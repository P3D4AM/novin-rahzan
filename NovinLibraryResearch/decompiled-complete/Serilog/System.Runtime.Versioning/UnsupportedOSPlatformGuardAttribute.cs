using System.Diagnostics;

namespace System.Runtime.Versioning;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
[Conditional("MULTI_TARGETING_SUPPORT_ATTRIBUTES")]
internal sealed class UnsupportedOSPlatformGuardAttribute : Attribute
{
	public UnsupportedOSPlatformGuardAttribute(string platformName)
	{
	}
}
