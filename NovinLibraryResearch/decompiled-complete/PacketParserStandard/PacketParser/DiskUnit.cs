namespace PacketParser;

public static class DiskUnit
{
	public static string ToUnit(this long value)
	{
		if (value < 1024)
		{
			return $"{value} B";
		}
		if (value < 1048576)
		{
			return $"{value / 1024} KB";
		}
		if (value < 1073741824)
		{
			return $"{value / 1048576} MB";
		}
		if (value < 1099511627776L)
		{
			return $"{value / 1073741824} GB";
		}
		if (value < 1125899906842624L)
		{
			return $"{value / 1099511627776L} TB";
		}
		return value.ToString();
	}
}
