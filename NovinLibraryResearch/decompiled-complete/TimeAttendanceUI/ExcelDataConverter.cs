using System.Globalization;

public static class ExcelDataConverter
{
	public static long ToLong(string input, long defaultValue = 0L)
	{
		if (!long.TryParse(input, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static int ToInt(string input, int defaultValue = 0)
	{
		if (!int.TryParse(input, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static short ToShort(string input, short defaultValue = 0)
	{
		if (!short.TryParse(input, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static double ToDouble(string input, double defaultValue = 0.0)
	{
		if (!double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static decimal ToDecimal(string input, decimal defaultValue = 0m)
	{
		if (!decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static bool ToBool(string input, bool defaultValue = false)
	{
		if (!bool.TryParse(input, out var result))
		{
			return defaultValue;
		}
		return result;
	}
}
