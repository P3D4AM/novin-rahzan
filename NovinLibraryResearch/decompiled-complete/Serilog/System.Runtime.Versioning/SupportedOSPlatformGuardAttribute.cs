using System.Diagnostics;

namespace System.Runtime.Versioning;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
[Conditional("MULTI_TARGETING_SUPPORT_ATTRIBUTES")]
internal sealed class SupportedOSPlatformGuardAttribute : Attribute
{
	public SupportedOSPlatformGuardAttribute(string platformName)
	{
	}
}
