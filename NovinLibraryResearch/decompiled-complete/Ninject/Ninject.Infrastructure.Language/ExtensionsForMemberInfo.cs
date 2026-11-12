using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Ninject.Infrastructure.Language;

public static class ExtensionsForMemberInfo
{
	private const BindingFlags DefaultFlags = BindingFlags.Instance | BindingFlags.Public;

	private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

	private static MethodInfo parentDefinitionMethodInfo;

	private static MethodInfo ParentDefinitionMethodInfo
	{
		get
		{
			if (parentDefinitionMethodInfo == null)
			{
				parentDefinitionMethodInfo = typeof(MethodInfo).Assembly.GetType("System.Reflection.RuntimeMethodInfo").GetMethod("GetParentDefinition", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return parentDefinitionMethodInfo;
		}
	}

	public static bool HasAttribute<T>(this MemberInfo member)
	{
		return member.HasAttribute(typeof(T));
	}

	public static bool HasAttribute(this MemberInfo member, Type type)
	{
		if (member is PropertyInfo element)
		{
			return IsDefined(element, type, inherit: true);
		}
		return member.IsDefined(type, inherit: true);
	}

	public static PropertyInfo GetPropertyFromDeclaredType(this MemberInfo memberInfo, PropertyInfo propertyDefinition, BindingFlags flags)
	{
		return memberInfo.DeclaringType.GetProperty(propertyDefinition.Name, flags, null, propertyDefinition.PropertyType, (from parameter in propertyDefinition.GetIndexParameters()
			select parameter.ParameterType).ToArray(), null);
	}

	public static bool IsPrivate(this PropertyInfo propertyInfo)
	{
		MethodInfo getMethod = propertyInfo.GetGetMethod(nonPublic: true);
		MethodInfo setMethod = propertyInfo.GetSetMethod(nonPublic: true);
		if (getMethod == null || getMethod.IsPrivate)
		{
			if (!(setMethod == null))
			{
				return setMethod.IsPrivate;
			}
			return true;
		}
		return false;
	}

	public static object[] GetCustomAttributesExtended(this MemberInfo member, Type attributeType, bool inherited)
	{
		return Attribute.GetCustomAttributes(member, attributeType, inherited);
	}

	private static PropertyInfo GetParentDefinition(PropertyInfo property)
	{
		MethodInfo methodInfo = property.GetGetMethod(nonPublic: true) ?? property.GetSetMethod(nonPublic: true);
		if (methodInfo != null)
		{
			methodInfo = methodInfo.GetParentDefinition(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (methodInfo != null)
			{
				return methodInfo.GetPropertyFromDeclaredType(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			}
		}
		return null;
	}

	private static MethodInfo GetParentDefinition(this MethodInfo method, BindingFlags flags)
	{
		if (ParentDefinitionMethodInfo == null)
		{
			return null;
		}
		return (MethodInfo)ParentDefinitionMethodInfo.Invoke(method, flags, null, null, CultureInfo.InvariantCulture);
	}

	private static bool IsDefined(PropertyInfo element, Type attributeType, bool inherit)
	{
		if (element.IsDefined(attributeType, inherit))
		{
			return true;
		}
		if (inherit)
		{
			if (!InternalGetAttributeUsage(attributeType).Inherited)
			{
				return false;
			}
			PropertyInfo parentDefinition = GetParentDefinition(element);
			while (parentDefinition != null)
			{
				if (parentDefinition.IsDefined(attributeType, inherit: false))
				{
					return true;
				}
				parentDefinition = GetParentDefinition(parentDefinition);
			}
		}
		return false;
	}

	private static object[] GetCustomAttributes(PropertyInfo propertyInfo, Type attributeType, bool inherit)
	{
		if (inherit && InternalGetAttributeUsage(attributeType).Inherited)
		{
			Dictionary<Type, bool> attributeUsages = new Dictionary<Type, bool>();
			List<object> list = new List<object>();
			list.AddRange(propertyInfo.GetCustomAttributes(attributeType, inherit: false));
			PropertyInfo parentDefinition = GetParentDefinition(propertyInfo);
			while (parentDefinition != null)
			{
				object[] customAttributes = parentDefinition.GetCustomAttributes(attributeType, inherit: false);
				AddAttributes(list, customAttributes, attributeUsages);
				parentDefinition = GetParentDefinition(parentDefinition);
			}
			object[] array = Array.CreateInstance(attributeType, list.Count) as object[];
			Array.Copy(list.ToArray(), array, array.Length);
			return array;
		}
		return propertyInfo.GetCustomAttributes(attributeType, inherit);
	}

	private static void AddAttributes(List<object> attributes, object[] customAttributes, Dictionary<Type, bool> attributeUsages)
	{
		foreach (object obj in customAttributes)
		{
			Type type = obj.GetType();
			if (!attributeUsages.ContainsKey(type))
			{
				attributeUsages[type] = InternalGetAttributeUsage(type).Inherited;
			}
			if (attributeUsages[type])
			{
				attributes.Add(obj);
			}
		}
	}

	private static AttributeUsageAttribute InternalGetAttributeUsage(Type type)
	{
		return type.GetCustomAttribute<AttributeUsageAttribute>(inherit: true);
	}
}
