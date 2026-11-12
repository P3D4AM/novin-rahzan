using System;

namespace Stimulsoft.Base.Serializing;

[AttributeUsage(AttributeTargets.Property)]
public sealed class StiDefaultStringValueAttribute : Attribute
{
	public string Value { get; set; }

	public StiDefaultStringValueAttribute(string value)
	{
		Value = value;
	}
}
