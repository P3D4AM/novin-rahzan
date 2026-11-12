using System;
using System.Reflection;

namespace Microsoft.Extensions.Internal;

internal static class ClosedGenericMatcher
{
	public static Type ExtractGenericInterface(Type queryType, Type interfaceType)
	{
		if (queryType == null)
		{
			throw new ArgumentNullException("queryType");
		}
		if (interfaceType == null)
		{
			throw new ArgumentNullException("interfaceType");
		}
		if (IsGenericInstantiation(queryType, interfaceType))
		{
			return queryType;
		}
		return GetGenericInstantiation(queryType, interfaceType);
	}

	private static bool IsGenericInstantiation(Type candidate, Type interfaceType)
	{
		if (candidate.GetTypeInfo().IsGenericType)
		{
			return candidate.GetGenericTypeDefinition() == interfaceType;
		}
		return false;
	}

	private static Type GetGenericInstantiation(Type queryType, Type interfaceType)
	{
		Type type = null;
		Type[] interfaces = queryType.GetInterfaces();
		foreach (Type type2 in interfaces)
		{
			if (IsGenericInstantiation(type2, interfaceType))
			{
				if (type == null)
				{
					type = type2;
				}
				else if (StringComparer.Ordinal.Compare(type2.FullName, type.FullName) < 0)
				{
					type = type2;
				}
			}
		}
		if (type != null)
		{
			return type;
		}
		Type type3 = queryType?.GetTypeInfo().BaseType;
		if (type3 == null)
		{
			return null;
		}
		return GetGenericInstantiation(type3, interfaceType);
	}
}
