using System;
using System.IO;

namespace SharpCompress.Compressors.LZMA;

internal static class Crc
{
	internal const uint INIT_CRC = uint.MaxValue;

	internal static readonly uint[] TABLE;

	static Crc()
	{
		TABLE = new uint[1024];
		for (uint num = 0u; num < 256; num++)
		{
			uint num2 = num;
			for (int i = 0; i < 8; i++)
			{
				num2 = (num2 >> 1) ^ (0xEDB88320u & ~((num2 & 1) - 1));
			}
			TABLE[num] = num2;
		}
		for (uint num3 = 256u; num3 < TABLE.Length; num3++)
		{
			uint num4 = TABLE[num3 - 256];
			TABLE[num3] = TABLE[num4 & 0xFF] ^ (num4 >> 8);
		}
	}

	public static uint From(Stream stream, long length)
	{
		uint crc = uint.MaxValue;
		byte[] array = new byte[Math.Min(length, 4096L)];
		while (length > 0)
		{
			int num = stream.Read(array, 0, (int)Math.Min(length, array.Length));
			if (num == 0)
			{
				throw new EndOfStreamException();
			}
			crc = Update(crc, array, 0, num);
			length -= num;
		}
		return Finish(crc);
	}

	public static uint Finish(uint crc)
	{
		return ~crc;
	}

	public static uint Update(uint crc, byte bt)
	{
		return TABLE[(crc & 0xFF) ^ bt] ^ (crc >> 8);
	}

	public static uint Update(uint crc, uint value)
	{
		crc ^= value;
		return TABLE[768 + (crc & 0xFF)] ^ TABLE[512 + ((crc >> 8) & 0xFF)] ^ TABLE[256 + ((crc >> 16) & 0xFF)] ^ TABLE[crc >> 24];
	}

	public static uint Update(uint crc, ulong value)
	{
		return Update(Update(crc, (uint)value), (uint)(value >> 32));
	}

	public static uint Update(uint crc, long value)
	{
		return Update(crc, (ulong)value);
	}

	public static uint Update(uint crc, byte[] buffer, int offset, int length)
	{
		for (int i = 0; i < length; i++)
		{
			crc = Update(crc, buffer[offset + i]);
		}
		return crc;
	}
}
