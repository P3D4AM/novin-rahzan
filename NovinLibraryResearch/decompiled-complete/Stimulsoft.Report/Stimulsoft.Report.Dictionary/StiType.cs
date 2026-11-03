using System;
using System.Drawing;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiType
{
	public string Name { get; set; }

	public Type Type { get; set; }

	public static StiTypesCollection GetTypes()
	{
		StiTypesCollection stiTypesCollection = new StiTypesCollection();
		foreach (Type simpleType in StiTypeWrapper.SimpleTypes)
		{
			stiTypesCollection.Add(new StiType(StiTypeConverter.ToString(simpleType), simpleType));
		}
		return stiTypesCollection;
	}

	public static StiTypesCollection GetBaseTypes()
	{
		StiTypesCollection stiTypesCollection = new StiTypesCollection();
		foreach (Type simpleBaseType in StiTypeWrapper.SimpleBaseTypes)
		{
			stiTypesCollection.Add(new StiType(StiTypeConverter.ToString(simpleBaseType), simpleBaseType));
		}
		return stiTypesCollection;
	}

	public static Type GetTypeModeFromType(Type type, out StiTypeMode typeMode)
	{
		typeMode = StiTypeMode.Value;
		if (type == null)
		{
			return type;
		}
		if (type == typeof(string))
		{
			return type;
		}
		if (type == typeof(bool))
		{
			return type;
		}
		if (type == typeof(byte))
		{
			return type;
		}
		if (type == typeof(sbyte))
		{
			return type;
		}
		if (type == typeof(ushort))
		{
			return type;
		}
		if (type == typeof(short))
		{
			return type;
		}
		if (type == typeof(uint))
		{
			return type;
		}
		if (type == typeof(int))
		{
			return type;
		}
		if (type == typeof(ulong))
		{
			return type;
		}
		if (type == typeof(long))
		{
			return type;
		}
		if (type == typeof(float))
		{
			return type;
		}
		if (type == typeof(double))
		{
			return type;
		}
		if (type == typeof(decimal))
		{
			return type;
		}
		if (type == typeof(char))
		{
			return type;
		}
		if (type == typeof(TimeSpan))
		{
			return type;
		}
		if (type == typeof(DateTime))
		{
			return type;
		}
		if (type == typeof(Guid))
		{
			return type;
		}
		if (type == typeof(Image))
		{
			return type;
		}
		if (type == typeof(object))
		{
			return type;
		}
		typeMode = StiTypeMode.NullableValue;
		if (type == typeof(bool?))
		{
			return typeof(bool);
		}
		if (type == typeof(byte?))
		{
			return typeof(byte);
		}
		if (type == typeof(sbyte?))
		{
			return typeof(sbyte);
		}
		if (type == typeof(ushort?))
		{
			return typeof(ushort);
		}
		if (type == typeof(short?))
		{
			return typeof(short);
		}
		if (type == typeof(uint?))
		{
			return typeof(uint);
		}
		if (type == typeof(int?))
		{
			return typeof(int);
		}
		if (type == typeof(ulong?))
		{
			return typeof(ulong);
		}
		if (type == typeof(long?))
		{
			return typeof(long);
		}
		if (type == typeof(float?))
		{
			return typeof(float);
		}
		if (type == typeof(double?))
		{
			return typeof(double);
		}
		if (type == typeof(decimal?))
		{
			return typeof(decimal);
		}
		if (type == typeof(char?))
		{
			return typeof(char);
		}
		if (type == typeof(TimeSpan?))
		{
			return typeof(TimeSpan);
		}
		if (type == typeof(DateTime?))
		{
			return typeof(DateTime);
		}
		if (type == typeof(Guid?))
		{
			return typeof(Guid);
		}
		typeMode = StiTypeMode.List;
		if (type == typeof(StringList))
		{
			return typeof(string);
		}
		if (type == typeof(BoolList))
		{
			return typeof(bool);
		}
		if (type == typeof(ByteList))
		{
			return typeof(byte);
		}
		if (type == typeof(ShortList))
		{
			return typeof(short);
		}
		if (type == typeof(IntList))
		{
			return typeof(int);
		}
		if (type == typeof(LongList))
		{
			return typeof(long);
		}
		if (type == typeof(FloatList))
		{
			return typeof(float);
		}
		if (type == typeof(DoubleList))
		{
			return typeof(double);
		}
		if (type == typeof(DecimalList))
		{
			return typeof(decimal);
		}
		if (type == typeof(CharList))
		{
			return typeof(char);
		}
		if (type == typeof(TimeSpanList))
		{
			return typeof(TimeSpan);
		}
		if (type == typeof(DateTimeList))
		{
			return typeof(DateTime);
		}
		if (type == typeof(GuidList))
		{
			return typeof(Guid);
		}
		typeMode = StiTypeMode.Range;
		if (type == typeof(StringRange))
		{
			return typeof(string);
		}
		if (type == typeof(ByteRange))
		{
			return typeof(byte);
		}
		if (type == typeof(ShortRange))
		{
			return typeof(short);
		}
		if (type == typeof(IntRange))
		{
			return typeof(int);
		}
		if (type == typeof(LongRange))
		{
			return typeof(long);
		}
		if (type == typeof(FloatRange))
		{
			return typeof(float);
		}
		if (type == typeof(DoubleRange))
		{
			return typeof(double);
		}
		if (type == typeof(DecimalRange))
		{
			return typeof(decimal);
		}
		if (type == typeof(CharRange))
		{
			return typeof(char);
		}
		if (type == typeof(TimeSpanRange))
		{
			return typeof(TimeSpan);
		}
		if (type == typeof(DateTimeRange))
		{
			return typeof(DateTime);
		}
		if (type == typeof(GuidRange))
		{
			return typeof(Guid);
		}
		typeMode = StiTypeMode.Value;
		return type;
	}

	public static Type GetTypeFromTypeMode(Type type, StiTypeMode typeMode)
	{
		if (type == null)
		{
			return null;
		}
		switch (typeMode)
		{
		case StiTypeMode.Value:
			return type;
		case StiTypeMode.NullableValue:
			if (type == typeof(bool))
			{
				return typeof(bool?);
			}
			if (type == typeof(byte))
			{
				return typeof(byte?);
			}
			if (type == typeof(sbyte))
			{
				return typeof(sbyte?);
			}
			if (type == typeof(ushort))
			{
				return typeof(ushort?);
			}
			if (type == typeof(short))
			{
				return typeof(short?);
			}
			if (type == typeof(uint))
			{
				return typeof(uint?);
			}
			if (type == typeof(int))
			{
				return typeof(int?);
			}
			if (type == typeof(ulong))
			{
				return typeof(ulong?);
			}
			if (type == typeof(long))
			{
				return typeof(long?);
			}
			if (type == typeof(float))
			{
				return typeof(float?);
			}
			if (type == typeof(double))
			{
				return typeof(double?);
			}
			if (type == typeof(decimal))
			{
				return typeof(decimal?);
			}
			if (type == typeof(char))
			{
				return typeof(char?);
			}
			if (type == typeof(TimeSpan))
			{
				return typeof(TimeSpan?);
			}
			if (type == typeof(DateTime))
			{
				return typeof(DateTime?);
			}
			if (type == typeof(Guid))
			{
				return typeof(Guid?);
			}
			break;
		case StiTypeMode.List:
			if (type == typeof(string))
			{
				return typeof(StringList);
			}
			if (type == typeof(bool))
			{
				return typeof(BoolList);
			}
			if (type == typeof(byte))
			{
				return typeof(ByteList);
			}
			if (type == typeof(short))
			{
				return typeof(ShortList);
			}
			if (type == typeof(int))
			{
				return typeof(IntList);
			}
			if (type == typeof(long))
			{
				return typeof(LongList);
			}
			if (type == typeof(float))
			{
				return typeof(FloatList);
			}
			if (type == typeof(double))
			{
				return typeof(DoubleList);
			}
			if (type == typeof(decimal))
			{
				return typeof(DecimalList);
			}
			if (type == typeof(char))
			{
				return typeof(CharList);
			}
			if (type == typeof(TimeSpan))
			{
				return typeof(TimeSpanList);
			}
			if (type == typeof(DateTime))
			{
				return typeof(DateTimeList);
			}
			if (type == typeof(Guid))
			{
				return typeof(GuidList);
			}
			break;
		case StiTypeMode.Range:
			if (type == typeof(string))
			{
				return typeof(StringRange);
			}
			if (type == typeof(byte))
			{
				return typeof(ByteRange);
			}
			if (type == typeof(short))
			{
				return typeof(ShortRange);
			}
			if (type == typeof(int))
			{
				return typeof(IntRange);
			}
			if (type == typeof(long))
			{
				return typeof(LongRange);
			}
			if (type == typeof(float))
			{
				return typeof(FloatRange);
			}
			if (type == typeof(double))
			{
				return typeof(DoubleRange);
			}
			if (type == typeof(decimal))
			{
				return typeof(DecimalRange);
			}
			if (type == typeof(char))
			{
				return typeof(CharRange);
			}
			if (type == typeof(TimeSpan))
			{
				return typeof(TimeSpanRange);
			}
			if (type == typeof(DateTime))
			{
				return typeof(DateTimeRange);
			}
			if (type == typeof(Guid))
			{
				return typeof(GuidRange);
			}
			break;
		}
		return type;
	}

	public override string ToString()
	{
		return Name;
	}

	public StiType(string name, Type type)
	{
		Name = name;
		Type = type;
	}
}
