using System;

namespace Stimulsoft.Base.Design;

[AttributeUsage(AttributeTargets.Property)]
public sealed class StiPropertyLevelAttribute : Attribute
{
	public StiLevel Level { get; }

	public StiPropertyLevelAttribute()
		: this((StiLevel)2)
	{
	}

	public StiPropertyLevelAttribute(StiLevel level)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		Level = level;
	}
}
