using System;

namespace Microsoft.SqlServer.Management.Assessment;

internal static class Names
{
	public const string AttributeContextName = "attr";

	public const string NamespaceSeparator = "::";

	public const char PropertySeparator = '.';

	public static bool SplitIf(string ns, string name, out string section, out string value, out string tail)
	{
		int num = name.IndexOf("::", StringComparison.InvariantCulture);
		if (num == ns.Length && name.StartsWith(ns, StringComparison.InvariantCulture))
		{
			int num2 = name.IndexOf("::", num + "::".Length, StringComparison.InvariantCulture);
			if (0 < num2 && num2 < name.Length - 1)
			{
				int num3 = name.IndexOf('.', num2 + "::".Length);
				if (num3 < 0)
				{
					value = name.Substring(num2 + "::".Length);
					tail = string.Empty;
				}
				else
				{
					value = name.Substring(num2 + "::".Length, num3 - num2 - "::".Length);
					tail = name.Substring(num3 + 1);
				}
				section = name.Substring(num + "::".Length, num2 - num - "::".Length);
				return true;
			}
		}
		section = null;
		value = null;
		tail = null;
		return false;
	}

	public static bool SplitIf(string ns, string name, out string tail)
	{
		int num = name.IndexOf("::", StringComparison.InvariantCulture);
		if (num == ns.Length && name.StartsWith(ns, StringComparison.InvariantCulture))
		{
			tail = name.Substring(num + "::".Length);
		}
		tail = null;
		return false;
	}

	public static bool Split(string ns, string name, out string tail)
	{
		int num = name.IndexOf("::", StringComparison.InvariantCulture);
		if (num < 0)
		{
			tail = name;
			return true;
		}
		if (num == ns.Length && name.StartsWith(ns, StringComparison.InvariantCulture))
		{
			tail = name.Substring(num + "::".Length);
			return true;
		}
		tail = null;
		return false;
	}

	public static bool Split(this string name, out string ns, out string tail)
	{
		int num = name.IndexOf("::", StringComparison.InvariantCulture);
		if (num >= 0)
		{
			ns = name.Substring(0, num);
			tail = name.Substring(num + "::".Length);
			return true;
		}
		ns = string.Empty;
		tail = name;
		return false;
	}
}
