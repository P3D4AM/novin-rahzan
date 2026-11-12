using System;
using System.Reflection;

namespace Ninject.Infrastructure.Language;

internal static class ExtensionsForICustomAttributeProvider
{
	public static bool HasAttribute(this ICustomAttributeProvider member, Type type)
	{
		if (member is MemberInfo member2)
		{
			return member2.HasAttribute(type);
		}
		return member.IsDefined(type, inherit: true);
	}

	public static object[] GetCustomAttributesExtended(this ICustomAttributeProvider member, Type attributeType, bool inherit)
	{
		if (member is MemberInfo member2)
		{
			return member2.GetCustomAttributesExtended(attributeType, inherit);
		}
		return member.GetCustomAttributes(attributeType, inherit);
	}
}
