using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Configuration;

public class ConfigurationKeyComparer : IComparer<string>
{
	private static readonly string[] _keyDelimiterArray = new string[1] { ConfigurationPath.KeyDelimiter };

	public static ConfigurationKeyComparer Instance { get; } = new ConfigurationKeyComparer();


	internal static Comparison<string> Comparison { get; } = Instance.Compare;


	public int Compare(string x, string y)
	{
		string[] array = x?.Split(_keyDelimiterArray, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
		string[] array2 = y?.Split(_keyDelimiterArray, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
		for (int i = 0; i < Math.Min(array.Length, array2.Length); i++)
		{
			x = array[i];
			y = array2[i];
			int result = 0;
			int result2 = 0;
			bool flag = x != null && int.TryParse(x, out result);
			bool flag2 = y != null && int.TryParse(y, out result2);
			int num = ((!flag && !flag2) ? string.Compare(x, y, StringComparison.OrdinalIgnoreCase) : ((!(flag && flag2)) ? ((!flag) ? 1 : (-1)) : (result - result2)));
			if (num != 0)
			{
				return num;
			}
		}
		return array.Length - array2.Length;
	}
}
