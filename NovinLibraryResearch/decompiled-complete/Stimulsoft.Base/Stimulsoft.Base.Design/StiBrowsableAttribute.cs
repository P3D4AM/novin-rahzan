using System;

namespace Stimulsoft.Base.Design;

[AttributeUsage(AttributeTargets.Property)]
public sealed class StiBrowsableAttribute : Attribute
{
	public bool Browsable { get; }

	public StiBrowsableAttribute()
		: this(browsable: true)
	{
	}

	public StiBrowsableAttribute(bool browsable)
	{
		Browsable = browsable;
	}
}
