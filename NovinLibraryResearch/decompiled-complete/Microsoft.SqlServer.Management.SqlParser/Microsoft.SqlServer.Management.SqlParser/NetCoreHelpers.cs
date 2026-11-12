using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Microsoft.SqlServer.Management.SqlParser;

internal static class NetCoreHelpers
{
	public static int InvariantCultureLcid => CultureInfo.InvariantCulture.LCID;

	public static XmlWriter CreateXmlWriter(TextWriter textWriter, XmlWriterSettings xmlSettings)
	{
		return XmlWriter.Create(textWriter, xmlSettings);
	}

	public static StringComparer GetStringComparer(this CultureInfo culture, bool ignoreCase)
	{
		return StringComparer.Create(culture, ignoreCase);
	}

	public static Assembly GetAssembly(this Type type)
	{
		return type.Assembly;
	}

	public static Type GetBaseType(this Type type)
	{
		return type.BaseType;
	}

	public static Type[] GetGenericArguments(this Type type)
	{
		return type.GetGenericArguments();
	}

	public static bool GetIsAssignableFrom(this Type type, Type c)
	{
		return type.IsAssignableFrom(c);
	}

	public static bool GetIsClass(this Type type)
	{
		return type.IsClass;
	}

	public static bool GetIsEnum(this Type type)
	{
		return type.IsEnum;
	}

	public static bool GetIsGenericType(this Type type)
	{
		return type.IsGenericType;
	}

	public static bool GetIsNestedPrivate(this Type type)
	{
		return type.IsNestedPrivate;
	}

	public static bool GetIsPrimitive(this Type type)
	{
		return type.IsPrimitive;
	}

	public static bool GetIsValueType(this Type type)
	{
		return type.IsValueType;
	}
}
