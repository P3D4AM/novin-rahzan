using System.Linq;

namespace System.Reflection;

internal static class MemberInfoExtensions
{
	public static Type GetMemberType(this MemberInfo memberInfo)
	{
		object obj = (memberInfo as PropertyInfo)?.PropertyType;
		if (obj == null)
		{
			FieldInfo obj2 = (FieldInfo)memberInfo;
			if ((object)obj2 == null)
			{
				return null;
			}
			obj = obj2.FieldType;
		}
		return (Type)obj;
	}

	public static bool IsSameAs(this MemberInfo propertyInfo, MemberInfo otherPropertyInfo)
	{
		if (!(propertyInfo == null))
		{
			if (!(otherPropertyInfo == null))
			{
				if (!object.Equals(propertyInfo, otherPropertyInfo))
				{
					if (propertyInfo.Name == otherPropertyInfo.Name)
					{
						if (!(propertyInfo.DeclaringType == otherPropertyInfo.DeclaringType) && !propertyInfo.DeclaringType.GetTypeInfo().IsSubclassOf(otherPropertyInfo.DeclaringType) && !otherPropertyInfo.DeclaringType.GetTypeInfo().IsSubclassOf(propertyInfo.DeclaringType) && !propertyInfo.DeclaringType.GetTypeInfo().ImplementedInterfaces.Contains<Type>(otherPropertyInfo.DeclaringType))
						{
							return otherPropertyInfo.DeclaringType.GetTypeInfo().ImplementedInterfaces.Contains<Type>(propertyInfo.DeclaringType);
						}
						return true;
					}
					return false;
				}
				return true;
			}
			return false;
		}
		return otherPropertyInfo == null;
	}

	public static string GetSimpleMemberName(this MemberInfo member)
	{
		string name = member.Name;
		int num = name.LastIndexOf('.');
		if (num < 0)
		{
			return name;
		}
		return name.Substring(num + 1);
	}
}
