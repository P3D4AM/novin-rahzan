using System;
using System.Reflection;
using System.Resources;

namespace Microsoft.SqlServer.Management.Common;

internal static class CommonDisplayKeyHelper
{
	private static CommonDisplayNameKeyAttribute displayNameKey = new CommonDisplayNameKeyAttribute(" ");

	private static CommonDisplayDescriptionKeyAttribute displayDiscKey = new CommonDisplayDescriptionKeyAttribute(" ");

	private static CommonDisplayCategoryKeyAttribute displayCatKey = new CommonDisplayCategoryKeyAttribute(" ");

	private static ICommonDisplayKey GetDisplayKey(Type keyAttribute)
	{
		ICommonDisplayKey result = null;
		if (keyAttribute.Equals(typeof(CommonDisplayNameKeyAttribute)))
		{
			result = displayNameKey;
		}
		else if (keyAttribute.Equals(typeof(CommonDisplayDescriptionKeyAttribute)))
		{
			result = displayDiscKey;
		}
		else if (keyAttribute.Equals(typeof(CommonDisplayCategoryKeyAttribute)))
		{
			result = displayCatKey;
		}
		return result;
	}

	public static string GetValueFromCustomAttribute(FieldInfo field, Type keyAttribute, ResourceManager resourceManager, bool isDefault)
	{
		string text = null;
		if (isDefault)
		{
			return GetDisplayValue(GetDisplayKey(keyAttribute).GetDefaultKey(field), resourceManager);
		}
		return GetCustomDisplayValue(field.GetCustomAttributes(keyAttribute, inherit: true), resourceManager);
	}

	public static string GetValueFromCustomAttribute(PropertyInfo property, Type keyAttribute, ResourceManager resourceManager, bool isDefault)
	{
		string text = null;
		if (isDefault)
		{
			return GetDisplayValue(GetDisplayKey(keyAttribute).GetDefaultKey(property), resourceManager);
		}
		return GetCustomDisplayValue(property.GetCustomAttributes(keyAttribute, inherit: true), resourceManager);
	}

	public static string GetValueFromCustomAttribute(Type type, Type keyAttribute, ResourceManager resourceManager, bool isDefault)
	{
		string text = null;
		if (isDefault)
		{
			return GetDisplayValue(GetDisplayKey(keyAttribute).GetDefaultKey(type), resourceManager);
		}
		return GetCustomDisplayValue(type.GetCustomAttributes(keyAttribute, inherit: true), resourceManager);
	}

	private static string GetDisplayValue(string key, ResourceManager resourceManager)
	{
		string result = null;
		if (resourceManager != null)
		{
			result = resourceManager.GetString(key);
		}
		return result;
	}

	private static string GetCustomDisplayValue(object[] customAttributes, ResourceManager resourceManager)
	{
		string result = null;
		if (customAttributes != null && 0 < customAttributes.GetLength(0) && resourceManager != null)
		{
			string key = ((ICommonDisplayKey)customAttributes[0]).Key;
			result = resourceManager.GetString(key);
		}
		return result;
	}

	public static string ConvertNullToEmptyString(string value)
	{
		if (value != null)
		{
			return value;
		}
		return string.Empty;
	}

	public static string ConstructDefaultKey(string postfix, string delim, PropertyInfo property)
	{
		return property.DeclaringType.Name + delim + property.Name + postfix;
	}

	public static string ConstructDefaultKey(string postfix, string delim, Type type)
	{
		return type.Name + delim + postfix;
	}

	public static string ConstructDefaultKey(string postfix, string delim, FieldInfo field)
	{
		return field.DeclaringType.Name + delim + field.MemberType.ToString() + delim + field.Name + postfix;
	}
}
