using System.Globalization;

namespace ExcelDataReader.Core;

internal static class ReferenceHelper
{
	public static bool ParseReference(string value, out int column, out int row)
	{
		column = 0;
		int num = 0;
		if (value != null)
		{
			while (num < value.Length)
			{
				char c = value[num];
				if (c >= 'A' && c <= 'Z')
				{
					num++;
					column *= 26;
					column += c - 64;
					continue;
				}
				if (!char.IsDigit(c))
				{
					num = 0;
				}
				break;
			}
		}
		if (num == 0)
		{
			column = 0;
			row = 0;
			return false;
		}
		if (!int.TryParse(value.Substring(num), NumberStyles.None, CultureInfo.InvariantCulture, out row))
		{
			return false;
		}
		return true;
	}
}
