using System;

namespace SharpCompress.Compressors.Xz;

internal static class Crc32
{
	public const uint DefaultPolynomial = 3988292384u;

	public const uint DefaultSeed = uint.MaxValue;

	private static uint[] defaultTable;

	public static uint Compute(byte[] buffer)
	{
		return Compute(uint.MaxValue, buffer);
	}

	public static uint Compute(uint seed, byte[] buffer)
	{
		return Compute(3988292384u, seed, buffer);
	}

	public static uint Compute(uint polynomial, uint seed, byte[] buffer)
	{
		return ~CalculateHash(InitializeTable(polynomial), seed, buffer);
	}

	private static uint[] InitializeTable(uint polynomial)
	{
		if (polynomial == 3988292384u && defaultTable != null)
		{
			return defaultTable;
		}
		uint[] array = new uint[256];
		for (int i = 0; i < 256; i++)
		{
			uint num = (uint)i;
			for (int j = 0; j < 8; j++)
			{
				num = (((num & 1) != 1) ? (num >> 1) : ((num >> 1) ^ polynomial));
			}
			array[i] = num;
		}
		if (polynomial == 3988292384u)
		{
			defaultTable = array;
		}
		return array;
	}

	private static uint CalculateHash(uint[] table, uint seed, ReadOnlySpan<byte> buffer)
	{
		uint num = seed;
		int length = buffer.Length;
		for (int i = 0; i < length; i++)
		{
			num = (num >> 8) ^ table[(buffer[i] ^ num) & 0xFF];
		}
		return num;
	}
}
