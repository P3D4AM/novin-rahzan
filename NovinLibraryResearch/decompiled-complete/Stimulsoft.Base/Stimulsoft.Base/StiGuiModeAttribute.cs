using System;

namespace Stimulsoft.Base;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false)]
public class StiGuiModeAttribute : Attribute
{
	public StiGuiMode Mode { get; }

	public StiGuiModeAttribute(StiGuiMode mode)
	{
		Mode = mode;
	}
}
