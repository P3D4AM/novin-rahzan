using System;
using System.Reflection;

namespace Stimulsoft.Base.Databases;

public class StiTableProperty
{
	public string Name { get; set; }

	public Type Type { get; set; }

	public PropertyInfo PropertyInfo { get; }

	public bool IsJson => PropertyInfo == null;

	public bool IsTableField => PropertyInfo.GetCustomAttributes(typeof(StiTableFieldAttribute), inherit: true).Length != 0;

	public override string ToString()
	{
		return Name;
	}

	public StiTableProperty()
	{
	}

	public StiTableProperty(PropertyInfo prop)
	{
		Name = prop.Name;
		Type = prop.PropertyType;
		PropertyInfo = prop;
	}
}
