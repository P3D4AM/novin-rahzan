using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Stimulsoft.Base;

public class StiTypeWrapper
{
	public static List<Type> SimpleTypes;

	public static List<Type> SimpleBaseTypes;

	public static List<Type> SimpleNullableTypes;

	public Type Type { get; }

	public override string ToString()
	{
		return StiTypeConverter.ToString(Type);
	}

	public static string ToString(Type type)
	{
		return StiTypeConverter.ToString(type);
	}

	public static StiTypeWrapper[] GetTypeWrappers()
	{
		return (from t in SimpleTypes.ToList()
			select new StiTypeWrapper(t)).ToArray();
	}

	public static StiTypeWrapper[] GetBaseTypeWrappers()
	{
		return (from t in SimpleBaseTypes.ToList()
			select new StiTypeWrapper(t)).ToArray();
	}

	public static bool IsAllowedType(Type type)
	{
		return SimpleTypes.Any((Type t) => t == type);
	}

	public StiTypeWrapper(Type type)
	{
		Type = type;
	}

	static StiTypeWrapper()
	{
		SimpleBaseTypes = new List<Type>
		{
			typeof(string),
			typeof(float),
			typeof(double),
			typeof(decimal),
			typeof(DateTime),
			typeof(TimeSpan),
			typeof(sbyte),
			typeof(byte),
			typeof(byte[]),
			typeof(short),
			typeof(ushort),
			typeof(int),
			typeof(uint),
			typeof(long),
			typeof(ulong),
			typeof(bool),
			typeof(char),
			typeof(Guid),
			typeof(object),
			typeof(Image)
		};
		SimpleNullableTypes = new List<Type>
		{
			typeof(float?),
			typeof(double?),
			typeof(decimal?),
			typeof(DateTime?),
			typeof(TimeSpan?),
			typeof(sbyte?),
			typeof(byte?),
			typeof(short?),
			typeof(ushort?),
			typeof(int?),
			typeof(uint?),
			typeof(long?),
			typeof(ulong?),
			typeof(bool?),
			typeof(char?),
			typeof(Guid?)
		};
		SimpleTypes = SimpleBaseTypes.Concat(SimpleNullableTypes).ToList();
	}
}
