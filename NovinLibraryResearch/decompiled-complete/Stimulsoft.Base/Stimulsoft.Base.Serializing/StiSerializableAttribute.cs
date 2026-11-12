using System;

namespace Stimulsoft.Base.Serializing;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public sealed class StiSerializableAttribute : Attribute
{
	public StiSerializationVisibility Visibility { get; }

	public StiSerializeTypes SerializeType { get; set; }

	public StiSerializableAttribute()
		: this(StiSerializationVisibility.Content)
	{
	}

	public StiSerializableAttribute(StiSerializationVisibility visibility)
		: this(visibility, StiSerializeTypes.SerializeToAll)
	{
	}

	public StiSerializableAttribute(StiSerializeTypes serializeType)
		: this(StiSerializationVisibility.Content, serializeType)
	{
	}

	public StiSerializableAttribute(StiSerializationVisibility visibility, StiSerializeTypes serializeType)
	{
		SerializeType = serializeType;
		Visibility = visibility;
	}
}
