using System;
using System.Reflection;

namespace Stimulsoft.Report.Dashboard;

internal class StiInvokeMethodsHelper
{
	public static object InvokeStaticMethod(string assemblyName, string className, string methodName)
	{
		return InvokeStaticMethod(assemblyName, className, methodName, null, null);
	}

	public static object InvokeStaticMethod(string assemblyName, string className, string methodName, object[] parameters)
	{
		return InvokeStaticMethod(assemblyName, className, methodName, parameters, null);
	}

	public static object InvokeStaticMethod(string assemblyName, string className, string methodName, object[] parameters, Type[] parametersTypes)
	{
		Type type = Type.GetType(assemblyName + "." + className + ", " + assemblyName + ", Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
		if (type == null)
		{
			return null;
		}
		BindingFlags bindingAttr = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
		MethodInfo methodInfo = ((parametersTypes != null) ? type.GetMethod(methodName, bindingAttr, null, parametersTypes, null) : type.GetMethod(methodName, bindingAttr));
		if (methodInfo != null)
		{
			return methodInfo.Invoke(null, parameters);
		}
		return null;
	}

	public static void SetPropertyValue(object obj, string propertyName, object value)
	{
		obj?.GetType().GetProperty(propertyName)?.SetValue(obj, value, null);
	}

	public static object GetPropertyValue(object obj, string propertyName)
	{
		if (obj == null)
		{
			return null;
		}
		PropertyInfo property = obj.GetType().GetProperty(propertyName);
		if (!(property != null))
		{
			return null;
		}
		return property.GetValue(obj, null);
	}

	public static Type GetType(string assemblyName, string className)
	{
		return Type.GetType(assemblyName + "." + className + ", " + assemblyName + ", Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
	}
}
