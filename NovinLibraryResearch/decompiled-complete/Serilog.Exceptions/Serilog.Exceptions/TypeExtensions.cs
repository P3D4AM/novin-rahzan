using System;

namespace Serilog.Exceptions;

internal static class TypeExtensions
{
	public static TypeCode GetTypeCode(this Type type)
	{
		return Type.GetTypeCode(type);
	}
}
