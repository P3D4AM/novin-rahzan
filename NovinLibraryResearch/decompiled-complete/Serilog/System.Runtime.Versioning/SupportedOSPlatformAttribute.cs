using System.Diagnostics;

namespace System.Runtime.Versioning;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
[Conditional("MULTI_TARGETING_SUPPORT_ATTRIBUTES")]
internal sealed class SupportedOSPlatformAttribute : Attribute
{
	public SupportedOSPlatformAttribute(string platformName)
	{
	}
}
