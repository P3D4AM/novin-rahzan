using System;
using System.Reflection;

namespace Stimulsoft.Base;

public class StiUndefinedType : TypeDelegator
{
	public string Type { get; }

	public override Type BaseType => typeof(object);

	public override Type UnderlyingSystemType => typeof(object);

	public override string Name => Type;

	public override string FullName => Type;

	protected override bool IsValueTypeImpl()
	{
		return false;
	}

	protected override bool IsArrayImpl()
	{
		return false;
	}

	protected override TypeAttributes GetAttributeFlagsImpl()
	{
		return TypeAttributes.NotPublic;
	}

	public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
	{
		return new PropertyInfo[0];
	}

	public override Type[] GetInterfaces()
	{
		return new Type[0];
	}

	public override string ToString()
	{
		return Type;
	}

	public StiUndefinedType(string type)
	{
		Type = type;
	}
}
