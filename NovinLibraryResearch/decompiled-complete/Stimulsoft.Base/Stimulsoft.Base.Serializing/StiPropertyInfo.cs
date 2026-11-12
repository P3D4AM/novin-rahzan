using System;

namespace Stimulsoft.Base.Serializing;

public class StiPropertyInfo
{
	public StiPropertyInfo Parent { get; set; }

	public string Name { get; set; }

	public object Value { get; set; }

	public object DefaultValue { get; set; }

	public bool IsDefaultValueSpecified { get; set; }

	public bool IsKey { get; set; }

	public bool IsReference { get; set; }

	public bool IsSerializable { get; set; }

	public bool IsList { get; set; }

	public int Count { get; set; }

	public int ReferenceCode { get; set; }

	public string TypeName { get; set; }

	public Type Type
	{
		get
		{
			if (Value == null)
			{
				return typeof(object);
			}
			return Value.GetType();
		}
	}

	public StiPropertyInfoCollection Properties { get; set; }

	public override string ToString()
	{
		return Name;
	}

	public StiPropertyInfo(string name, object value, object defaultValue, bool isDefaultValueSpecified, bool isKey, bool isReference, bool isList, string typeName)
	{
		Properties = new StiPropertyInfoCollection(this);
		Name = name;
		Value = value;
		DefaultValue = defaultValue;
		IsDefaultValueSpecified = isDefaultValueSpecified;
		IsKey = isKey;
		IsReference = isReference;
		IsList = isList;
		ReferenceCode = -1;
		TypeName = typeName;
	}

	public StiPropertyInfo(string name, object value, bool isKey, bool isReference, bool isList, string typeName)
		: this(name, value, null, isDefaultValueSpecified: false, isKey, isReference, isList, typeName)
	{
	}
}
