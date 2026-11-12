using System;
using System.Reflection;

namespace DevComponents.DotNetBar.Design;

internal static class ReflectionHelpers
{
	public static object GetValueProperty(object owner, string propertyName, BindingFlags flags)
	{
		if (owner == null)
		{
			return null;
		}
		PropertyInfo property = owner.GetType().GetProperty(propertyName, flags);
		return GetValueProperty(owner, property);
	}

	public static object GetValueProperty(object owner, PropertyInfo propertyInfo)
	{
		if (owner == null || (object)propertyInfo == null)
		{
			return null;
		}
		return propertyInfo.GetValue(owner, null);
	}

	public static PropertyInfo GetPropertyInfo(object owner, string propertyName, BindingFlags flags)
	{
		if (owner == null)
		{
			return null;
		}
		return GetPropertyInfo(owner.GetType(), propertyName, flags);
	}

	public static object InvokeMethod(object owner, string nameMethod, BindingFlags flags, object[] args)
	{
		MethodInfo methodInfo = GetMethodInfo(owner, nameMethod, flags);
		return InvokeMethod(owner, methodInfo, args);
	}

	public static MethodInfo GetMethodInfo(object owner, string nameMethod, BindingFlags flags)
	{
		if (owner == null)
		{
			return null;
		}
		return GetMethodInfo(owner.GetType(), nameMethod, flags);
	}

	public static MethodInfo GetMethodInfo(Type type, string nameMethod, BindingFlags flags)
	{
		return type?.GetMethod(nameMethod, flags);
	}

	public static object InvokeMethod(object owner, MethodInfo methodInfo, object[] args)
	{
		if ((object)methodInfo == null || owner == null)
		{
			return null;
		}
		return methodInfo.Invoke(owner, args);
	}

	public static void SetValueField(object owner, string nameField, BindingFlags flags, object value)
	{
		FieldInfo fieldInfo = GetFieldInfo(owner, nameField, flags);
		SetValueField(owner, fieldInfo, value);
	}

	public static void SetValueField(object owner, FieldInfo fieldInfo, object value)
	{
		if (owner != null)
		{
			fieldInfo?.SetValue(owner, value);
		}
	}

	public static FieldInfo GetFieldInfo(Type type, string nameField, BindingFlags flags)
	{
		return type?.GetField(nameField, flags);
	}

	public static FieldInfo GetFieldInfo(object owner, string nameField, BindingFlags flags)
	{
		if (owner == null)
		{
			return null;
		}
		return GetFieldInfo(owner.GetType(), nameField, flags);
	}

	public static ConstructorInfo GetConstructorInfo(Type type, Type[] args)
	{
		return type?.GetConstructor(args);
	}

	public static object GetValueField(object owner, string nameField, BindingFlags flags)
	{
		return owner?.GetType().GetField(nameField, flags)?.GetValue(owner);
	}
}
