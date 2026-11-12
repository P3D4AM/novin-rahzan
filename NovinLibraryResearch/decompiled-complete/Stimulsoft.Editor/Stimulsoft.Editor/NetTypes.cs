using System;
using System.Collections;
using System.Reflection;

namespace Stimulsoft.Editor;

public class NetTypes
{
	private static Hashtable assemblies = new Hashtable();

	private static Hashtable types = new Hashtable();

	private static Hashtable namespaces = new Hashtable();

	private static Hashtable globalTypes = new Hashtable();

	public static Hashtable Assemblies => assemblies;

	public static Hashtable Types => types;

	public static Hashtable GlobalTypes => globalTypes;

	public static Hashtable Namespaces => namespaces;

	protected static string GetGenericName(Type type)
	{
		string text = type.FullName;
		if (type.IsGenericType)
		{
			int num = text.IndexOf('`');
			if (num >= 0)
			{
				text = text.Substring(0, num);
			}
		}
		return text;
	}

	public static object GetTypeByName(string name, IList assemblies, bool caseSensitive)
	{
		object obj = types[name];
		if (obj != null)
		{
			if (obj == null || !assemblies.Contains(((Type)obj).Assembly))
			{
				return null;
			}
			return obj;
		}
		try
		{
			obj = Type.GetType(name, throwOnError: false, !caseSensitive);
			if (obj == null)
			{
				foreach (Assembly assembly in assemblies)
				{
					obj = assembly.GetType(name, throwOnError: false, !caseSensitive);
					if (obj != null && !IsSpecialType((Type)obj))
					{
						break;
					}
				}
			}
		}
		catch
		{
		}
		return obj;
	}

	public static IList GetNamespaces(IList assemblies)
	{
		IList list = new ArrayList();
		IDictionaryEnumerator enumerator = namespaces.GetEnumerator();
		enumerator.Reset();
		while (enumerator.MoveNext())
		{
			Hashtable hashtable = (Hashtable)enumerator.Value;
			foreach (Assembly assembly in assemblies)
			{
				if (hashtable.Contains(assembly))
				{
					list.Add(enumerator.Key);
					break;
				}
			}
		}
		return list;
	}

	public static int GetGlobalTypes(IList types, string nspace, bool caseSensitive)
	{
		types.Clear();
		foreach (Type key in globalTypes.Keys)
		{
			if (string.Compare(key.Namespace, nspace, !caseSensitive) == 0)
			{
				types.Add(key);
			}
		}
		return types.Count;
	}

	public static void AddType(Type type, bool global)
	{
		types[GetGenericName(type)] = type;
		if (global)
		{
			globalTypes[type] = type;
		}
	}

	public static void RemoveType(Type type)
	{
		string genericName = GetGenericName(type);
		if (types.Contains(genericName))
		{
			types.Remove(genericName);
		}
		if (globalTypes.Contains(type))
		{
			globalTypes.Remove(type);
		}
	}

	public static void AddNamespace(string nspace, Assembly assembly)
	{
		object obj = namespaces[nspace];
		Hashtable hashtable = ((obj != null) ? ((Hashtable)obj) : new Hashtable());
		hashtable[assembly] = assembly;
		if (obj == null)
		{
			namespaces[nspace] = hashtable;
		}
	}

	public static void RemoveNamespace(string nspace, Assembly assembly)
	{
		object obj = namespaces[nspace];
		if (obj != null)
		{
			Hashtable hashtable = (Hashtable)obj;
			if (hashtable.Contains(assembly))
			{
				hashtable.Remove(assembly);
			}
			if (hashtable.Count == 0)
			{
				namespaces.Remove(nspace);
			}
		}
	}

	public static bool IsSpecialType(Type type)
	{
		if (!type.IsSpecialName && !type.IsNotPublic && !type.IsNestedPublic && !type.IsNestedPrivate && !type.IsNestedAssembly && !type.IsNestedFamily && !type.Name.StartsWith("$$") && !type.Name.StartsWith("<"))
		{
			return type.Name.StartsWith("__");
		}
		return true;
	}

	public static void AddAssembly(Assembly assembly)
	{
		if (assemblies.Contains(assembly))
		{
			return;
		}
		assemblies[assembly] = assembly;
		try
		{
			Type[] array = assembly.GetTypes();
			foreach (Type type in array)
			{
				if (!IsSpecialType(type))
				{
					AddType(type, global: false);
					if (type.Namespace != null && type.Namespace != string.Empty)
					{
						AddNamespace(type.Namespace, assembly);
					}
				}
			}
		}
		catch
		{
		}
	}

	public static void RemoveAssembly(Assembly assembly)
	{
		if (!assemblies.Contains(assembly))
		{
			return;
		}
		try
		{
			Type[] array = assembly.GetTypes();
			foreach (Type type in array)
			{
				if (!type.IsSpecialName && !type.Name.StartsWith("_"))
				{
					RemoveType(type);
					if (type.Namespace != null && type.Namespace != string.Empty)
					{
						RemoveNamespace(type.Namespace, assembly);
					}
				}
			}
		}
		catch
		{
		}
		assemblies.Remove(assembly);
	}
}
