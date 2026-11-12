using System;
using System.Text;

namespace Microsoft.Identity.Client.Utils;

internal static class StringExtensions
{
	public static byte[] ToByteArray(this string stringInput)
	{
		return new UTF8Encoding().GetBytes(stringInput);
	}

	public static string Replace(this string src, string oldValue, string newValue, StringComparison comparison)
	{
		if (string.IsNullOrWhiteSpace(src))
		{
			return src;
		}
		if (string.IsNullOrWhiteSpace(oldValue))
		{
			throw new ArgumentException("oldValue cannot be empty");
		}
		if (string.Compare(oldValue, newValue, comparison) == 0)
		{
			return src;
		}
		if (oldValue.Length > src.Length)
		{
			return src;
		}
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		int num2;
		for (num2 = src.IndexOf(oldValue, comparison); num2 != -1; num2 = src.IndexOf(oldValue, num2, comparison))
		{
			stringBuilder.Append(src.Substring(num, num2 - num));
			stringBuilder.Append(newValue);
			num2 += oldValue.Length;
			num = num2;
		}
		stringBuilder.Append(src.Substring(num));
		return stringBuilder.ToString();
	}
}
