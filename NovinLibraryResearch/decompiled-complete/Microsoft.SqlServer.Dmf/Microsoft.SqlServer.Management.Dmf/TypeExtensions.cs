using System;

namespace Microsoft.SqlServer.Management.Dmf;

internal static class TypeExtensions
{
	public static bool IsBitmappedEnum(this Type t)
	{
		object[] customAttributes = t.GetCustomAttributes(typeof(FlagsAttribute), inherit: true);
		if (t.IsEnum)
		{
			return customAttributes.Length != 0;
		}
		return false;
	}
}
