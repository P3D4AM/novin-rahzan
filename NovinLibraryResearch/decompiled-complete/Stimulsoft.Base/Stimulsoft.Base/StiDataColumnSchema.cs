using System;

namespace Stimulsoft.Base;

public class StiDataColumnSchema : StiObjectSchema
{
	public Type Type { get; set; }

	public StiDataColumnSchema()
	{
	}

	public StiDataColumnSchema(string name, Type type)
	{
		base.Name = name;
		Type = type;
	}
}
