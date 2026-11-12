using System;
using System.IO;

namespace SharpCompress.Compressors.ADC;

public static class ADCBase
{
	private const int PLAIN = 1;

	private const int TWO_BYTE = 2;

	private const int THREE_BYTE = 3;

	private static int GetChunkType(byte byt)
	{
		if ((byt & 0x80) == 128)
		{
			return 1;
		}
		if ((byt & 0x40) == 64)
		{
			return 3;
		}
		return 2;
	}

	private static int GetChunkSize(byte byt)
	{
		return GetChunkType(byt) switch
		{
			1 => (byt & 0x7F) + 1, 
			2 => ((byt & 0x3F) >> 2) + 3, 
			3 => (byt & 0x3F) + 4, 
			_ => -1, 
		};
	}

	private static int GetOffset(ReadOnlySpan<byte> chunk)
	{
		return GetChunkType(chunk[0]) switch
		{
			1 => 0, 
			2 => ((chunk[0] & 3) << 8) + chunk[1], 
			3 => (chunk[1] << 8) + chunk[2], 
			_ => -1, 
		};
	}

	public static int Decompress(byte[] input, out byte[]? output, int bufferSize = 262144)
	{
		return Decompress(new MemoryStream(input), out output, bufferSize);
	}

	public static int Decompress(Stream input, out byte[]? output, int bufferSize = 262144)
	{
		output = null;
		if (input == null || input.Length == 0L)
		{
			return 0;
		}
		int num = (int)input.Position;
		int num2 = (int)input.Position;
		byte[] array = new byte[bufferSize];
		int num3 = 0;
		bool flag = false;
		Span<byte> span = stackalloc byte[3];
		while (num2 < input.Length)
		{
			int num4 = input.ReadByte();
			if (num4 == -1)
			{
				break;
			}
			switch (GetChunkType((byte)num4))
			{
			case 1:
			{
				int chunkSize = GetChunkSize((byte)num4);
				if (num3 + chunkSize > bufferSize)
				{
					flag = true;
					break;
				}
				input.Read(array, num3, chunkSize);
				num3 += chunkSize;
				num2 += chunkSize + 1;
				break;
			}
			case 2:
			{
				int chunkSize = GetChunkSize((byte)num4);
				span[0] = (byte)num4;
				span[1] = (byte)input.ReadByte();
				int offset = GetOffset(span);
				if (num3 + chunkSize > bufferSize)
				{
					flag = true;
				}
				else if (offset == 0)
				{
					byte b2 = array[num3 - 1];
					for (int k = 0; k < chunkSize; k++)
					{
						array[num3] = b2;
						num3++;
					}
					num2 += 2;
				}
				else
				{
					for (int l = 0; l < chunkSize; l++)
					{
						array[num3] = array[num3 - offset - 1];
						num3++;
					}
					num2 += 2;
				}
				break;
			}
			case 3:
			{
				int chunkSize = GetChunkSize((byte)num4);
				span[0] = (byte)num4;
				span[1] = (byte)input.ReadByte();
				span[2] = (byte)input.ReadByte();
				int offset = GetOffset(span);
				if (num3 + chunkSize > bufferSize)
				{
					flag = true;
				}
				else if (offset == 0)
				{
					byte b = array[num3 - 1];
					for (int i = 0; i < chunkSize; i++)
					{
						array[num3] = b;
						num3++;
					}
					num2 += 3;
				}
				else
				{
					for (int j = 0; j < chunkSize; j++)
					{
						array[num3] = array[num3 - offset - 1];
						num3++;
					}
					num2 += 3;
				}
				break;
			}
			}
			if (flag)
			{
				break;
			}
		}
		output = new byte[num3];
		Array.Copy(array, output, num3);
		return num2 - num;
	}
}
