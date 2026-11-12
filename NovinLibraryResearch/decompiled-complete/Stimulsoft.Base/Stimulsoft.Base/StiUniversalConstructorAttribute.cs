using System;

namespace Stimulsoft.Base;

[AttributeUsage(AttributeTargets.Constructor)]
public sealed class StiUniversalConstructorAttribute : Attribute
{
	public string Name { get; set; }

	public StiUniversalConstructorAttribute(string name)
	{
		Name = name;
	}

	public StiUniversalConstructorAttribute()
		: this("")
	{
	}
}
