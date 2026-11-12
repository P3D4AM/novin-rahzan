using System;
using System.Linq;
using System.Reflection;

namespace Stimulsoft.Base;

public class Reflection
{
	public static object Create(Assembly assembly, string type, params object[] args)
	{
		return Activator.CreateInstance(assembly.GetType(type), args);
	}

	public static T InvokeMethod<T>(object value, string methodName, params object[] args)
	{
		return (T)InvokeMethod(value, methodName, args);
	}

	public static object InvokeMethod(object value, string methodName, params object[] args)
	{
		if (args != null && args.Length != 0)
		{
			Type[] types = args.Select((object a) => a.GetType()).ToArray();
			return value.GetType().GetMethod(methodName, types).Invoke(value, args);
		}
		return value.GetType().GetMethod(methodName).Invoke(value, args);
	}

	public static T GetPropertyValue<T>(object value, string propertyName)
	{
		return (T)GetPropertyValue(value, propertyName);
	}

	public static object GetPropertyValue(object value, string propertyName)
	{
		PropertyInfo property = value.GetType().GetProperty(propertyName);
		if (!(property != null))
		{
			return null;
		}
		return property.GetValue(value, null);
	}

	public static void SetPropertyValue(object value, string propertyName, object propValue)
	{
		value.GetType().GetProperty(propertyName).SetValue(value, propValue, null);
	}
}
