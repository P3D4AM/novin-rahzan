using System;

namespace SmartAssembly.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
public sealed class ForceObfuscateAttribute : Attribute
{
	public ForceObfuscateAttribute(bool useHashAsName = false)
	{
	}
}
