using System;

namespace Stimulsoft.Base;

public class StiDataParameterSchema : StiObjectSchema
{
	public Type Type { get; set; }

	public object Value { get; set; }

	public StiDataParameterSchema()
	{
	}

	public StiDataParameterSchema(string name, Type type)
	{
		base.Name = name;
		Type = type;
	}
}
