using System;

namespace Stimulsoft.Report.Components;

[AttributeUsage(AttributeTargets.Class)]
public sealed class StiToolboxAttribute : Attribute
{
	public bool Enable { get; set; }

	public StiToolboxAttribute(bool enable)
	{
		Enable = enable;
	}
}
