namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal static class SerializationUtils
{
	public static byte GetBitLength(int n)
	{
		return GetBitLength((uint)n);
	}

	public static byte GetBitLength(uint n)
	{
		byte b = 1;
		while ((n /= 2) != 0)
		{
			b++;
		}
		return b;
	}

	public static byte GetDigitCount(int n)
	{
		byte b = 1;
		while ((n /= 10) > 0)
		{
			b++;
		}
		return b;
	}
}
