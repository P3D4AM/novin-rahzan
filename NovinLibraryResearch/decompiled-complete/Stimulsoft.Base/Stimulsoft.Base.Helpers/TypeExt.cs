using System;
using System.Collections;

namespace Stimulsoft.Base.Helpers;

public static class TypeExt
{
	private static Hashtable numerics;

	private static Hashtable integers;

	public static bool IsNumericType(this Type type)
	{
		if (type == null)
		{
			return false;
		}
		return numerics.ContainsKey(type);
	}

	public static bool IsIntegerType(this Type type)
	{
		if (type == null)
		{
			return false;
		}
		return integers.ContainsKey(type);
	}

	public static bool IsDateType(this Type type)
	{
		if (type == null)
		{
			return false;
		}
		if (!(type == typeof(DateTime)) && !(type == typeof(DateTimeOffset)))
		{
			return type == typeof(TimeSpan);
		}
		return true;
	}

	public static bool IsStringType(this Type type)
	{
		if (type == null)
		{
			return false;
		}
		return type == typeof(string);
	}

	public static bool IsBooleanType(this Type type)
	{
		if (type == null)
		{
			return false;
		}
		return type == typeof(bool);
	}

	static TypeExt()
	{
		numerics = new Hashtable();
		integers = new Hashtable();
		numerics.Add(typeof(sbyte), true);
		numerics.Add(typeof(byte), true);
		numerics.Add(typeof(short), true);
		numerics.Add(typeof(ushort), true);
		numerics.Add(typeof(int), true);
		numerics.Add(typeof(uint), true);
		numerics.Add(typeof(long), true);
		numerics.Add(typeof(ulong), true);
		numerics.Add(typeof(float), true);
		numerics.Add(typeof(double), true);
		numerics.Add(typeof(decimal), true);
		integers.Add(typeof(sbyte), true);
		integers.Add(typeof(byte), true);
		integers.Add(typeof(short), true);
		integers.Add(typeof(ushort), true);
		integers.Add(typeof(int), true);
		integers.Add(typeof(uint), true);
		integers.Add(typeof(long), true);
		integers.Add(typeof(ulong), true);
	}
}
