using System;
using System.Collections;
using System.Reflection;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Base;

public sealed class StiTypeFinder
{
	internal static Hashtable TypeToString = new Hashtable();

	internal static Hashtable StringToType = new Hashtable();

	private static Hashtable FindTypes = new Hashtable();

	public static event StiTypeNotFoundEventHandler TypeNotFound;

	public static event EventHandler<StiTypeEventArgs> TypeResolve;

	public static Type GetType(string typeName)
	{
		lock (TypeToString)
		{
			if (StiTypeFinder.TypeResolve != null)
			{
				StiTypeEventArgs stiTypeEventArgs = new StiTypeEventArgs(typeName);
				StiTypeFinder.TypeResolve(null, stiTypeEventArgs);
				if (stiTypeEventArgs.Type != null)
				{
					return stiTypeEventArgs.Type;
				}
			}
			if (!string.IsNullOrEmpty(typeName))
			{
				Type type = StringToType[typeName] as Type;
				if (type != null)
				{
					return type;
				}
				type = Type.GetType(typeName);
				if (!(type == null))
				{
					if (StringToType[typeName] == null)
					{
						lock (StringToType)
						{
							StringToType[typeName] = type;
						}
					}
					return type;
				}
				string text = "Stimulsoft." + typeName;
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				Assembly[] array = assemblies;
				foreach (Assembly assembly in array)
				{
					type = assembly.GetType(typeName);
					if (type != null)
					{
						if (StringToType[typeName] == null)
						{
							StringToType[typeName] = type;
						}
						return type;
					}
				}
			}
			StiTypeNotFoundEventArgs stiTypeNotFoundEventArgs = new StiTypeNotFoundEventArgs(typeName);
			StiTypeFinder.TypeNotFound?.Invoke(typeName, stiTypeNotFoundEventArgs);
			return stiTypeNotFoundEventArgs.CreatedObject as Type;
		}
	}

	private static void AddTypeFF(Type exType, Type typeForFinding, bool result)
	{
		lock (FindTypes)
		{
			if (!(exType == null) && !(typeForFinding == null))
			{
				Hashtable hashtable = FindTypes[exType] as Hashtable;
				if (hashtable == null)
				{
					hashtable = new Hashtable();
					FindTypes[exType] = hashtable;
				}
				if (!hashtable.ContainsKey(typeForFinding))
				{
					hashtable[typeForFinding] = result;
				}
			}
		}
	}

	private static object GetTypeFF(Type exType, Type typeForFinding)
	{
		if (exType == null)
		{
			return null;
		}
		if (!(FindTypes[exType] is Hashtable hashtable))
		{
			return null;
		}
		return hashtable[typeForFinding];
	}

	public static bool FindType(Type exType, Type typeForFinding)
	{
		if (exType == null)
		{
			return false;
		}
		if (typeForFinding == typeof(object))
		{
			return true;
		}
		object typeFF = GetTypeFF(exType, typeForFinding);
		if (typeFF != null)
		{
			return (bool)typeFF;
		}
		while (exType != typeof(object))
		{
			if (exType == typeForFinding)
			{
				AddTypeFF(exType, typeForFinding, result: true);
				return true;
			}
			if (exType.BaseType == null)
			{
				AddTypeFF(exType, typeForFinding, result: false);
				return false;
			}
			exType = exType.BaseType;
		}
		AddTypeFF(exType, typeForFinding, result: false);
		return false;
	}

	public static bool FindInterface(Type exType, Type interfaceForFinding)
	{
		if (exType == null)
		{
			return false;
		}
		object typeFF = GetTypeFF(exType, interfaceForFinding);
		if (typeFF != null)
		{
			return (bool)typeFF;
		}
		while (exType != typeof(object))
		{
			if (exType == null)
			{
				AddTypeFF(exType, interfaceForFinding, result: false);
				return false;
			}
			Type[] interfaces = exType.GetInterfaces();
			Type[] array = interfaces;
			foreach (Type type in array)
			{
				if (type == interfaceForFinding)
				{
					AddTypeFF(exType, interfaceForFinding, result: true);
					return true;
				}
			}
			exType = exType.BaseType;
		}
		AddTypeFF(exType, interfaceForFinding, result: false);
		return false;
	}
}
