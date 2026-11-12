using System;

namespace Serilog.Exceptions.Reflection;

internal class ReflectionPropertyInfo
{
	private bool markedWithTypeName;

	public string Name { get; private set; }

	public Type? DeclaringType { get; }

	public Func<object, object> Getter { get; }

	public ReflectionPropertyInfo(string name, Type? declaringType, Func<object, object> getter)
	{
		Name = name;
		DeclaringType = declaringType;
		Getter = getter;
	}

	public void MarkNameWithTypeName()
	{
		if (!markedWithTypeName)
		{
			markedWithTypeName = true;
			Name = DeclaringType?.Name + "." + Name;
		}
	}

	public void MarkNameWithTypeNameIfRedefinesThatProperty(ReflectionPropertyInfo otherPropertyInfo)
	{
		if (otherPropertyInfo != this && !(DeclaringType == null) && !(otherPropertyInfo?.DeclaringType == null) && !(otherPropertyInfo?.Name != Name) && !markedWithTypeName && IsSubTypeOf(otherPropertyInfo.DeclaringType, DeclaringType))
		{
			MarkNameWithTypeName();
		}
	}

	private static bool IsSubTypeOf(Type possibleSubType, Type possibleBaseType)
	{
		return possibleSubType.IsSubclassOf(possibleBaseType);
	}
}
