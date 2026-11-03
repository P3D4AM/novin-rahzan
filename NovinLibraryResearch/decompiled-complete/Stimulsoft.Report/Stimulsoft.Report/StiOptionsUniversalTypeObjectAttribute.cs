using System;

namespace Stimulsoft.Report;

public class StiOptionsUniversalTypeObjectAttribute : Attribute
{
	public StiOptionsUniversalType UniversalType { get; }

	public string Type { get; }

	public StiOptionsUniversalTypeObjectAttribute(StiOptionsUniversalType universalType, string type)
	{
		Type = type;
		UniversalType = universalType;
	}
}
