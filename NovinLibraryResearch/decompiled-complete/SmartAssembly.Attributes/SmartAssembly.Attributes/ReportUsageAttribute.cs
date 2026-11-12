using System;

namespace SmartAssembly.Attributes;

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method)]
public sealed class ReportUsageAttribute : Attribute
{
	public ReportUsageAttribute()
	{
	}

	public ReportUsageAttribute(string featureName)
	{
	}
}
