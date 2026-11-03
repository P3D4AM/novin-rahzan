using System;

namespace Stimulsoft.Report.Components;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class StiContextToolAttribute : Attribute
{
	public Type Type { get; set; }

	public StiContextToolAttribute(Type type)
	{
		Type = type;
	}
}
