using System;
using System.Drawing;

namespace Stimulsoft.Base;

public static class StiTypeConverter
{
	public static string ToString(Type type)
	{
		if (type == null)
		{
			return "null";
		}
		if (type == typeof(byte[]))
		{
			return "byte[]";
		}
		if (type == typeof(Image))
		{
			return "image";
		}
		if (type == typeof(string))
		{
			return "string";
		}
		if (type == typeof(bool))
		{
			return "bool";
		}
		if (type == typeof(char))
		{
			return "char";
		}
		if (type == typeof(Guid))
		{
			return "guid";
		}
		if (type == typeof(DateTime))
		{
			return "datetime";
		}
		if (type == typeof(TimeSpan))
		{
			return "timespan";
		}
		if (type == typeof(float))
		{
			return "float";
		}
		if (type == typeof(double))
		{
			return "double";
		}
		if (type == typeof(decimal))
		{
			return "decimal";
		}
		if (type == typeof(sbyte))
		{
			return "sbyte";
		}
		if (type == typeof(short))
		{
			return "short";
		}
		if (type == typeof(int))
		{
			return "int";
		}
		if (type == typeof(long))
		{
			return "long";
		}
		if (type == typeof(byte))
		{
			return "byte";
		}
		if (type == typeof(ushort))
		{
			return "ushort";
		}
		if (type == typeof(uint))
		{
			return "uint";
		}
		if (type == typeof(ulong))
		{
			return "ulong";
		}
		if (type == typeof(bool?))
		{
			return "bool (Nullable)";
		}
		if (type == typeof(char?))
		{
			return "char (Nullable)";
		}
		if (type == typeof(Guid?))
		{
			return "guid (Nullable)";
		}
		if (type == typeof(DateTime?))
		{
			return "datetime (Nullable)";
		}
		if (type == typeof(TimeSpan?))
		{
			return "timespan (Nullable)";
		}
		if (type == typeof(float?))
		{
			return "float (Nullable)";
		}
		if (type == typeof(double?))
		{
			return "double (Nullable)";
		}
		if (type == typeof(decimal?))
		{
			return "decimal (Nullable)";
		}
		if (type == typeof(sbyte?))
		{
			return "sbyte (Nullable)";
		}
		if (type == typeof(short?))
		{
			return "short (Nullable)";
		}
		if (type == typeof(int?))
		{
			return "int (Nullable)";
		}
		if (type == typeof(long?))
		{
			return "long (Nullable)";
		}
		if (type == typeof(byte?))
		{
			return "byte (Nullable)";
		}
		if (type == typeof(ushort?))
		{
			return "ushort (Nullable)";
		}
		if (type == typeof(uint?))
		{
			return "uint (Nullable)";
		}
		if (type == typeof(ulong?))
		{
			return "ulong (Nullable)";
		}
		if (type == typeof(object))
		{
			return "object";
		}
		return type.ToString();
	}

	public static Type FromString(string type)
	{
		return type switch
		{
			"null" => null, 
			"byte[]" => typeof(byte[]), 
			"image" => typeof(Image), 
			"string" => typeof(string), 
			"bool" => typeof(bool), 
			"char" => typeof(char), 
			"guid" => typeof(Guid), 
			"datetime" => typeof(DateTime), 
			"timespan" => typeof(TimeSpan), 
			"float" => typeof(float), 
			"double" => typeof(double), 
			"decimal" => typeof(decimal), 
			"sbyte" => typeof(sbyte), 
			"short" => typeof(short), 
			"int" => typeof(int), 
			"long" => typeof(long), 
			"byte" => typeof(byte), 
			"ushort" => typeof(ushort), 
			"uint" => typeof(uint), 
			"ulong" => typeof(ulong), 
			"bool (Nullable)" => typeof(bool?), 
			"char (Nullable)" => typeof(char?), 
			"guid (Nullable)" => typeof(Guid?), 
			"datetime (Nullable)" => typeof(DateTime?), 
			"timespan (Nullable)" => typeof(TimeSpan?), 
			"float (Nullable)" => typeof(float?), 
			"double (Nullable)" => typeof(double?), 
			"decimal (Nullable)" => typeof(decimal?), 
			"sbyte (Nullable)" => typeof(sbyte?), 
			"short (Nullable)" => typeof(short?), 
			"int (Nullable)" => typeof(int?), 
			"long (Nullable)" => typeof(long?), 
			"byte (Nullable)" => typeof(byte?), 
			"ushort (Nullable)" => typeof(ushort?), 
			"uint (Nullable)" => typeof(uint?), 
			"ulong (Nullable)" => typeof(ulong?), 
			"object" => typeof(object), 
			_ => StiTypeFinder.GetType(type), 
		};
	}
}
