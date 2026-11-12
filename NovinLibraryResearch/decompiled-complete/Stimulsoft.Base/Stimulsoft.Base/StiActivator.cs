using System;

namespace Stimulsoft.Base;

public sealed class StiActivator
{
	public static bool AllowTypeNotFoundException;

	public static object CreateObject(Type type)
	{
		if (type == null)
		{
			return null;
		}
		return Activator.CreateInstance(type);
	}

	public static object CreateObject(string typeName)
	{
		return CreateObject(typeName, new object[0]);
	}

	public static object CreateObject(Type type, object[] arguments)
	{
		return CreateObject(type.ToString(), arguments);
	}

	public static object CreateObject(string typeName, object[] arguments)
	{
		Type type = StiTypeFinder.GetType(typeName);
		if (type != null)
		{
			try
			{
				return Activator.CreateInstance(type, arguments);
			}
			catch (Exception ex)
			{
				throw (ex.InnerException != null) ? ex.InnerException : ex;
			}
		}
		if (AllowTypeNotFoundException)
		{
			throw new Exception("The type " + typeName + " not found!");
		}
		return null;
	}
}
